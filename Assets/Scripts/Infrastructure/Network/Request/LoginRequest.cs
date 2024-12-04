using System;
using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class LoginRequest : ServerRequest
    {
        public long ReferralId { get; set; }
        public string InitData { get; set; }
        public string UserName { get; set; }
    }
}