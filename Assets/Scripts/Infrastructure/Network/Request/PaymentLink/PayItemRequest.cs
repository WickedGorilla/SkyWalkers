using System;
using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network.Request
{
    [Serializable]
    public class PayItemRequest : ServerRequest
    {
        public int ItemId;
        public int Amount;

        public PayItemRequest(int itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
        }
    }

    [Serializable]
    public class PayUpgradeRequest : ServerRequest
    {
        public int PerkId;
        public int Level;

        public PayUpgradeRequest(int perkId, int level)
        {
            PerkId = perkId;
            Level = level;
        }
    }
}