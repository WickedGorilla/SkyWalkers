using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Network.Request.Base;
using Infrastructure.Network.RequestHandler;

namespace Infrastructure.Network
{
    public interface IServerRequestSender
    {
        void Initialize(long userId);
        void UpdateToken(string token);
        
        public void AddHandler<T>(params IRequestHandler<T>[] handlers);
        public void RemoveHandler<T>(params IRequestHandler<T>[] handlers);

        UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action onError = null) where TRequest : ServerRequest;

        UniTask<ServerResponse<TResponse>> SendToServerAndHandle<TRequest, TResponse>(TRequest message,
            string address,
            Action onError = null) where TRequest : ServerRequest;

        void SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action<ServerResponse<TResponse>> onComplete, Action onError = null) where TRequest : ServerRequest;

        UniTask<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(TRequest message,
            string address, Action onError = null);
        
    }
}