using System;

namespace Infrastructure.Network.Request.Base
{
    [Serializable]
    public class ServerRequest
    {
        public long UserId;
        public string Token;
    }
}