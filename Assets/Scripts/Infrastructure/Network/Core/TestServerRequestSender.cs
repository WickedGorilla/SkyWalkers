using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Wallet.Flash;
using Infrastructure.Network.Response;

namespace Infrastructure.Network
{
    public class TestServerRequestSender : IServerRequestSender
    {
        private readonly Dictionary<Type, object> _fakeRequests;

        public TestServerRequestSender()
        {
            _fakeRequests = new Dictionary<Type, object>
            {
                [typeof(PlayerData)] = new PlayerData(64050567, new RangeValue(100, 100), 3, 3),
            };
        }

        public UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message, string address,
            Action onError = null)
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
    }
}