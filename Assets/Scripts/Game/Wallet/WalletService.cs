using System.Linq;
using Game.Wallet;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network.Response.Player;

namespace Player
{
    public class WalletService
    {
        public IntValue Coins = new();
        public IntRangeValue Energy = new(0, 0);
        public IntValue PlayPass = new();
        public IntValue Boosts = new();

        public void UpdateValues(BalanceUpdate update)
        {
            Coins.Update(update.Coins);
            PlayPass.Update(update.PlayPass);
            Boosts.Update(update.Boosts);
            Energy.Update(update.Energy);
        }

        public void UpdateValues(BalanceUpdate update, PerksResponse perks)
        {
            Coins.Update(update.Coins);
            PlayPass.Update(update.PlayPass);
            Boosts.Update(update.Boosts);
            
            var energyPerk = perks.Perks.FirstOrDefault(x => x.Id == (int)PerkType.EnergyLimit);
            var maxEnergy = energyPerk?.CurrentValue ?? 0;
            Energy.Update(update.Energy, maxEnergy);
        }
        
        public void UpdateValues(BalanceUpdate update, PerkInfo perkInfo)
        {
            Coins.Update(update.Coins);
            PlayPass.Update(update.PlayPass);
            Boosts.Update(update.Boosts);
            var maxEnergy = perkInfo.Id == (int)PerkType.EnergyLimit ? perkInfo.CurrentValue : Energy.Max;
            Energy.Update(update.Energy, maxEnergy);
        }
    }
}