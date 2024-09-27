using System;
using Game.Wallet.Flash;

namespace Infrastructure.Network.Response
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public RangeValue Energy;
        public int EnergyFlash;
        public int Boosts;

        public PlayerData(int coins, RangeValue energy, int energyFlash, int boosts)
        {
            Coins = coins;
            Energy = energy;
            EnergyFlash = energyFlash;
            Boosts = boosts;
        }
    }
}