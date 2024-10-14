using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Wallet.Flash;
using Infrastructure.Network.Response;
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
                [typeof(GameData)] = new GameData(UnityEngine.Random.Range(139402, 905945), new RangeValue(100, 100), 3, 3),
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

        public void SendToServer<TRequest, TResponse>(TRequest message, string address, Action<ServerResponse<TResponse>> onComplete, Action onError = null)
        {
            throw new NotImplementedException();
        }
    }
}