using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public struct BalanceUpdate
    {
        public int Coins;
        public int Energy;
        public int PlayPass;
        public int Boosts;
    }
}