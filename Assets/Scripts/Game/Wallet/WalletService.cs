using Game.Wallet;
using Infrastructure.Network.Response;

namespace Player
{
    public class WalletService
    {
        public IntValue Coins = new();
        public IntRangeValue Energy = new(0, 0);
        public IntValue EnergyFlash = new();
        public IntValue Boosts = new();

        public void Initialize(PlayerData data)
        {
            Coins = new IntValue(data.Coins);
            Energy = new IntRangeValue(data.Energy);
            EnergyFlash = new IntValue(data.EnergyFlash);
            Boosts = new IntValue(data.Boosts);
        }
    }
}