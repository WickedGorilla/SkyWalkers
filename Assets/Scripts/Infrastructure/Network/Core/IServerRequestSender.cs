using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Network
{
    public interface IServerRequestSender
    {
        UniTask<ServerResponse<TResponse>> SendToServer<TRequest, TResponse>(TRequest message,
            string address,
            Action onError = null);
    }
}