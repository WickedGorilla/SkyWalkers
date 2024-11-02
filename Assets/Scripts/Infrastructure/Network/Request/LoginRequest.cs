using System;
using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class LoginRequest : NetworkRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string AuthDate { get; set; }
        public string Hash { get; set; }
        
        public LoginRequest(string authDate, string hash)
        {
            AuthDate = authDate;
            Hash = hash;
            FirstName = string.Empty;
            LastName = string.Empty;
            UserName = string.Empty;
            PhotoUrl = string.Empty;
        }
    }
}