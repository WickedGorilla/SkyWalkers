using System.Linq;
using Game.Wallet;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.RequestHandler;
using Infrastructure.Network.Response.Player;

namespace Player
{
    public class WalletService : IRequestHandler<GameData>, IRequestHandler<ValidationPaymentResponse>
    {
        public IntValue Coins = new();
        public IntRangeValue Energy = new(0, 0);
        public IntValue PlayPass = new();
        public IntValue Boosts = new();

        public void Handle(GameData response)
        {
            UpdateValues(response.BalanceUpdate, response.Perks);
        }

        public void Handle(ValidationPaymentResponse response) 
            => UpdateValues(response.Balance, response.Perks);

        private void UpdateValues(BalanceUpdate update, PerksResponse perks)
        {
            Coins = new IntValue(update.Coins);
            PlayPass = new IntValue(update.PlayPass);
            Boosts = new IntValue(update.Boosts);
            
            var energyPerk = perks.Perks.FirstOrDefault(x => x.Id == (int)PerkType.EnergyLimit);
            var maxEnergy = energyPerk?.CurrentValue ?? 0;
            Energy = new IntRangeValue(update.Energy, maxEnergy);
        }
    }
}