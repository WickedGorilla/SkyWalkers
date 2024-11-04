using System;
using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network.Request.ValidationPayment
{
    [Serializable]
    public class ValidationPaymentRequest : ServerRequest
    {
        public string OrderCode;

        public ValidationPaymentRequest(string orderCode) 
            => OrderCode = orderCode;
    }
}