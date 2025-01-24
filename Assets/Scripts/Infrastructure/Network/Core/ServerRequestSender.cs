using System;
using System.Text;
using Infrastructure.Network.Request.Base;
using Infrastructure.Network.RequestHandler;
using Newtonsoft.Json;
using SkyExtensions.Awaitable;
using UnityEngine;
using UnityEngine.Networking;

namespace Infrastructure.Network
{
    public class ServerRequestSender : IServerRequestSender
    {
        private readonly string _baseUrl;
        private readonly ResponsesHandler _responsesHandler = new();

        private long _userId;
        private string _token;

        public ServerRequestSender()
        {
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
        {
            _userId = userId;
        }

        public void UpdateToken(string token)
        {
            _token = token;
        }

        public void AddHandler<T>(params IRequestHandler<T>[] handlers)
            => _responsesHandler.AddHandlers(handlers);

        public void RemoveHandler<T>(params IRequestHandler<T>[] handlers)
            => _responsesHandler.RemoveHandlers(handlers);

        public async Awaitable<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action<long, string> onError = null) where TRequest : ServerRequest
        {
            message.UserId = _userId;
            message.Token = _token;

            return await SendToServerBase<TRequest, TResponse>(message, address, onError);
        }

        public async Awaitable<ServerResponse<TResponse>> SendToServerAndHandle<TRequest, TResponse>
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

        public async void SendToServer<TRequest, TResponse>(TRequest message, string address,
            Action<ServerResponse<TResponse>> onComplete, Action<long, string> onError = null)
            where TRequest : ServerRequest
        {
            var response = await SendToServer<TRequest, TResponse>(message, address, onError);
            onComplete(response);
        }

        public async Awaitable<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(
            TRequest message,
            string address,
            Action<long, string> onError = null)
        {
            string jsonData = JsonConvert.SerializeObject(message);
            string authUrl = $"{_baseUrl}{address}";

            Debug.Log($"Send: {jsonData}");

            var request = new UnityWebRequest(authUrl, "POST");
            request.SetRequestHeader("Content-Type", "application/json");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SendWebRequest();
            await AwaitableExtensions.WaitUntilAsync(() => request.result != UnityWebRequest.Result.InProgress);

            var response = new ServerResponse<TResponse>();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var result = JsonConvert.DeserializeObject<TResponse>(request.downloadHandler.text);

                response.Success = result != null;
                response.Data = result;

                if (result == null)
                {
                    Debug.LogError("Failed to deserialize response: Result is null");
                    onError?.Invoke(request.responseCode, string.Empty);
                }
            }
            else
            {
                onError?.Invoke(request.responseCode, request.downloadHandler.text);
                Debug.LogError($"Response Failed: {request.error}");
            }

            return response;
        }
    }
}