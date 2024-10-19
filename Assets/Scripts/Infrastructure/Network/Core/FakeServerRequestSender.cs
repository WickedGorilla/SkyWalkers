using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Network.Request.Base;
using Infrastructure.Network.Response.Player;

namespace Infrastructure.Network
{
    public class FakeServerRequestSender : IServerRequestSender
    {
        private readonly Dictionary<Type, object> _fakeRequests;

        public FakeServerRequestSender()
        {
            _fakeRequests = new Dictionary<Type, object>
            {
                [typeof(GameData)] = new GameData(UnityEngine.Random.Range(139402, 905945), 100, 3, 3),
            };
        }


        public UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message, string address, Action onError = null) where TRequest : NetworkRequest
        {
            throw new NotImplementedException();
        }

        public void SendToServer<TRequest, TResponse>(TRequest message, string address, Action<ServerResponse<TResponse>> onComplete, Action onError = null) where TRequest : NetworkRequest
        {
            throw new NotImplementedException();
        }

        public UniTask<ServerResponse<TResponse>> SendToServerBase<TRequest, TResponse>(TRequest message, string address, Action onError = null)
        {
            if (!_fakeRequests.TryGetValue(typeof(TResponse), out object data))
                throw new KeyNotFoundException("No request was found");

            var response = new ServerResponse<TResponse>
            {
                Success = true,
                Data = (TResponse)data 
            };

            return new UniTask<ServerResponse<TResponse>>(response);
        }

        public void Initialize(long userId, string token)
        {
            
        }
    }
}