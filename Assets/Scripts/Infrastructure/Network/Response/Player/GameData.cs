using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class GameData
    {
        public string Token;
        public int Coins;
        public int Energy;
        public int PlayPass;
        public int Boosts;
        public PerksInfo PerksInfo;

        public GameData(int coins, int energy, int playPass, int boosts)
        {
            Coins = coins;
            Energy = energy;
            PlayPass = playPass;
            Boosts = boosts;
            PerksInfo = new PerksInfo();
        }
    }
}