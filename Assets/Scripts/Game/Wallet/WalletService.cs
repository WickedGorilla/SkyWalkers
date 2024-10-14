using Game.Wallet;
using Infrastructure.Network.Response;
using Infrastructure.Network.Response.Player;

namespace Player
{
    public class WalletService
    {
        public IntValue Coins = new();
        public IntRangeValue Energy = new(0, 0);
        public IntValue EnergyFlash = new();
        public IntValue Boosts = new();

        public void Initialize(GameData data)
        {
            Coins = new IntValue(data.Coins);
            Energy = new IntRangeValue(data.Energy);
            EnergyFlash = new IntValue(data.PlayPass);
            Boosts = new IntValue(data.Boosts);
        }
    }
}