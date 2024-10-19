using System;

namespace Infrastructure.Network.Request.Base
{
    [Serializable]
    public class NetworkRequest
    {
        public long UserId;
        public string Token;
    }
}