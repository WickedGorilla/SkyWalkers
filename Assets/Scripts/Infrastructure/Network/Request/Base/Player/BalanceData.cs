using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class BalanceData
    {
        public int Coins;
        public int Energy;
        public int PlayPass;
        public int Boosts;
    }
}