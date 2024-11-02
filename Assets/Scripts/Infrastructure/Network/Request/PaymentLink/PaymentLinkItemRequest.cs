using System;
using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class PaymentLinkItemRequest : NetworkRequest
    {
        public int ItemId;
        public int Amount;

        public PaymentLinkItemRequest(int itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
        }
    }

    [Serializable]
    public class PaymentLinkPerkUpgradeRequest : NetworkRequest
    {
        public int PerkId;
        public int Level;

        public PaymentLinkPerkUpgradeRequest(int perkId, int level)
        {
            PerkId = perkId;
            Level = level;
        }
    }
}