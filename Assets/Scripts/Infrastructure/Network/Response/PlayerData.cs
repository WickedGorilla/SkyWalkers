using System;

namespace Infrastructure.Network.Response
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Energy;
        public int Boosts;
    }
}