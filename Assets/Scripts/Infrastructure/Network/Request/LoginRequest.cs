using System;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class LoginRequest
    {
        public long UserId;
        public long AuthDate;
        public string Hash;
        
        public LoginRequest(long userId, long authDate, string hash)
        {
            UserId = userId;
            AuthDate = authDate;
            Hash = hash;
        }
    }
}