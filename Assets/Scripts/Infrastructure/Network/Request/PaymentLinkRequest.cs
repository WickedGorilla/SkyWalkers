using System;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class PaymentLinkRequest
    {
        public int UserId;
        public int ItemId;
        public int Amount;

        public PaymentLinkRequest(int userId, int itemId, int amount)
        {
            UserId = userId;
            ItemId = itemId;
            Amount = amount;
        }
    }
}