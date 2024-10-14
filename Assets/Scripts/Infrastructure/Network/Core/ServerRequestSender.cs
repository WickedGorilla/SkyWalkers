using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Infrastructure.Network
{
    public class ServerRequestSender : IServerRequestSender
    {
        private const string BaseUrl = "https://localhost:5128";

        private string _userId;

        public async UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action onError = null)
        {
            string jsonData = JsonUtility.ToJson(message);
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
                var result = JsonUtility.FromJson<TResponse>(request.downloadHandler.text);

                response.Success = result != null;
                response.Data = result;
            }
            else
            {
                onError?.Invoke();
                Debug.LogError($"Response Failed: {request.error}");
            }

            return response;
        }

        public async void SendToServer<TRequest, TResponse>(TRequest message, string address,
            Action<ServerResponse<TResponse>> onComplete, Action onError = null)
        {
            var response = await SendToServer<TRequest, TResponse>(message, address, onError);
            onComplete(response);
        }
    }
}