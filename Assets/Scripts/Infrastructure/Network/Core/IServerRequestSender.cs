using System;
using Infrastructure.Network.Request.Base;
using Infrastructure.Network.RequestHandler;
using UnityEngine;

namespace Infrastructure.Network
{
    public interface IServerRequestSender
    {
        void Initialize(long userId);
        void UpdateToken(string token);

        public void AddHandler<T>(params IRequestHandler<T>[] handlers);
        public void RemoveHandler<T>(params IRequestHandler<T>[] handlers);

        void SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action<ServerResponse<TResponse>> onComplete = null, Action<long, string> onError = null)
            where TRequest : ServerRequest;
        
        Awaitable<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action<long, string> onError = null) where TRequest : ServerRequest;
        
        void SendToServerAndHandle<TRequest, TResponse>
        (TRequest message, string address, Action<ServerResponse<TResponse>> onComplete,
            Action<long, string> onError = null) where TRequest : ServerRequest;

        Awaitable<ServerResponse<TResponse>> SendToServerAndHandle<TRequest, TResponse>(TRequest message,
            string address,
            Action<long, string> onError = null) where TRequest : ServerRequest;
        
        Awaitable<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(TRequest message,
            string address, Action<long, string> onError = null);
    }
}