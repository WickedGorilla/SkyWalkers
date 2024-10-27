using Game.Wallet;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.RequestHandler;
using Infrastructure.Network.Response.Player;

namespace Player
{
    public class WalletService : IRequestHandler<GameData>, IRequestHandler<ValidationPaymentResponse>
    {
        public IntValue Coins = new();
        public IntRangeValue Energy = new(0, 0);
        public IntValue EnergyFlash = new();
        public IntValue Boosts = new();

        public void Handle(GameData response)
        {
            var update = response.BalanceUpdate;
            
            Coins = new IntValue(update.Coins);
            Energy = new IntRangeValue(update.Energy, response.PerksInfo.EnergyLimit.CurrentValue);
            EnergyFlash = new IntValue(update.PlayPass);
            Boosts = new IntValue(update.Boosts);
        }

        public void Handle(ValidationPaymentResponse response)
        {
            if (!response.IsUpdated)
                return;
            
            var update = response.BalanceUpdate;
            Coins = new IntValue(update.Coins);
            Energy = new IntRangeValue(update.Energy, response.PerksInfo.EnergyLimit.CurrentValue);
            EnergyFlash = new IntValue(update.PlayPass);
            Boosts = new IntValue(update.Boosts);
        }
    }
}