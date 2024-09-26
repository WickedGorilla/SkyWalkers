using System;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class LoginRequest
    {
        public string UserId;
        public long Time;

        public LoginRequest(string userId)
        {
            UserId = userId;
        }
    }
}