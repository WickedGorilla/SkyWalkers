using System;
using Game.Wallet.Flash;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class GameData
    {
        public int Coins;
        public RangeValue Energy;
        public int PlayPass;
        public int Boosts;
        public PerkInfo[] PerksInfo;

        public GameData(int coins, RangeValue energy, int playPass, int boosts)
        {
            Coins = coins;
            Energy = energy;
            PlayPass = playPass;
            Boosts = boosts;
        }
    }
}