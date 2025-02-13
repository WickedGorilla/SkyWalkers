using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Infrastructure.Network.Request.Base;
using Infrastructure.Network.RequestHandler;
using Newtonsoft.Json;
using UI.Core;
using UI.Views.ServerErrorPopup;
using UnityEngine;
using UnityEngine.Networking;

namespace Infrastructure.Network
{
    public class ServerRequestSender : IServerRequestSender
    {
        private readonly ViewService _viewService;
        
        private readonly string _baseUrl;
        private readonly ResponsesHandler _responsesHandler = new();

        private long _userId;
        private string _token;

        private const long ConflictErrorCode = 409;

        public ServerRequestSender(ViewService viewService)
        {
            _viewService = viewService;

            _baseUrl =
#if DEV_BUILD
                "https://similarly-mutual-gobbler.ngrok-free.app/";
#elif DEV_BUILD_LOCAL
               "http://localhost:5000/";
#else
                "https://app.skywalkersgame.com/";
#endif
        }

        public void Initialize(long userId) 
            => _userId = userId;

        public void UpdateToken(string token) 
            => _token = token;

        public void AddHandler<T>(params IRequestHandler<T>[] handlers)
            => _responsesHandler.AddHandlers(handlers);

        public void RemoveHandler<T>(params IRequestHandler<T>[] handlers)
            => _responsesHandler.RemoveHandlers(handlers);

        public async void SendToServer<TRequest, TResponse>(TRequest message, string address,
            Action<ServerResponse<TResponse>> onComplete, Action<long, string> onError = null)
            where TRequest : ServerRequest
        {
            var response = await SendToServer<TRequest, TResponse>(message, address, onError);
            onComplete?.Invoke(response);
        }
        
        public async UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action<long, string> onError = null) where TRequest : ServerRequest
        {
            message.UserId = _userId;
            message.Token = _token;

            return await SendToServerBase<TRequest, TResponse>(message, address, onError);
        }

        public async void SendToServerAndHandle<TRequest, TResponse>
            (TRequest message, string address, Action<ServerResponse<TResponse>> onComplete, Action<long, string> onError = null) where TRequest : ServerRequest
        {
            var response = await SendToServerAndHandle<TRequest, TResponse>(message, address, onError);
            onComplete?.Invoke(response);
        }
        
        public async UniTask<ServerResponse<TResponse>> SendToServerAndHandle<TRequest, TResponse>
            (TRequest message, string address, Action<long, string> onError = null) where TRequest : ServerRequest
        {
            var result = await SendToServer<TRequest, TResponse>(message, address, onError);

            if (!result.Success)
            {
                return result;
            }

            _responsesHandler.GetHolder<TResponse>()?.HandleServerData(result.Data);
            return result;
        }

        public async UniTask<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(
            TRequest message,
            string address,
            Action<long, string> onError = null)
        {
            string jsonData = JsonConvert.SerializeObject(message);
            string authUrl = $"{_baseUrl}{address}";

#if DEV_BUILD
            Debug.Log($"Send: {jsonData}");
#endif

            var request = new UnityWebRequest(authUrl, "POST");
            request.SetRequestHeader("Content-Type", "application/json");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SendWebRequest();
            await UniTask.WaitUntil(() => request.result != UnityWebRequest.Result.InProgress);

            var response = new ServerResponse<TResponse>();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var result = JsonConvert.DeserializeObject<TResponse>(request.downloadHandler.text);

                response.Success = result != null;
                response.Data = result;

                if (result == null)
                    Debug.LogError("Failed to deserialize response: Result is null");
            }
            else
            {
                onError?.Invoke(request.responseCode, request.downloadHandler.text);
                OnResponseWithError(request.responseCode, request.downloadHandler.text);
                Debug.LogError($"Response Failed: {request.error}");
            }

            return response;
        }

        private void OnResponseWithError(long code, string data)
        {
            Debug.LogError($"Response with error: {code}");

            if (data == string.Empty)
                return;

            switch (code)
            {
                case ConflictErrorCode:

                    if (int.TryParse(data, out int errorCode))
                    {
                        _viewService.AddPopupToQueueAndShow<ServerErrorPopupView,
                            ServerErrorPopupController>(OnShowPopup);
                    }

                    void OnShowPopup(ServerErrorPopupController controller)
                        => controller.Initialize(errorCode);

                    return;
            }
        }
    }
}