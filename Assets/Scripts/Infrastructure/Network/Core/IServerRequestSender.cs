using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network
{
    public interface IServerRequestSender
    {
        UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action onError = null) where TRequest : NetworkRequest;

        void SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action<ServerResponse<TResponse>> onComplete, Action onError = null) where TRequest : NetworkRequest;

        UniTask<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(TRequest message,
            string address, Action onError = null);

        void Initialize(long userId, string token);
    }
}