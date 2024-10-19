using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Infrastructure.Network.Request.Base;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Infrastructure.Network
{
    public class ServerRequestSender : IServerRequestSender
    {
        private const string BaseUrl = "https://localhost:5128";

        private long _userId;
        private string _token;

        public void Initialize(long userId, string token)
        {
        }

        public async UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action onError = null) where TRequest : NetworkRequest
        {
            message.UserId = _userId;
            message.Token = _token;

            return await SendToServerBase<TRequest, TResponse>(message, address, onError);
        }

        public async void SendToServer<TRequest, TResponse>(TRequest message, string address,
            Action<ServerResponse<TResponse>> onComplete, Action onError = null) where TRequest : NetworkRequest
        {
            var response = await SendToServer<TRequest, TResponse>(message, address, onError);
            onComplete(response);
        }

        public async UniTask<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(
            TRequest message,
            string address,
            Action onError = null)
        {
            string jsonData = JsonConvert.SerializeObject(message);
            string authUrl = $"{BaseUrl}/{address}";

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
                {
                    Debug.LogError("Failed to deserialize response: Result is null");
                    onError?.Invoke();
                }
            }
            else
            {
                onError?.Invoke();
                Debug.LogError($"Response Failed: {request.error}");
            }

            return response;
        }
    }
}