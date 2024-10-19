using Infrastructure.Data.Game.Shop;
using Infrastructure.Network.Response.Player;

namespace Game.Perks
{
    public class PerksService
    {
        public PerkEntity EnergyLimit { get; private set; }
        public PerkEntity MultiTap { get;  private set; } 
        public PerkEntity AutoTap { get;  private set; }

        public void Initialize(PerksInfo perksInfo)
        {
            EnergyLimit = new PerkEntity(perksInfo.EnergyLimit, PerkType.EnergyLimit);
            MultiTap = new PerkEntity(perksInfo.MultiTap, PerkType.MultiTap);
            AutoTap = new PerkEntity(perksInfo.AutoTap, PerkType.AutoTap);
        }

        public PerkEntity GetPerkByType(PerkType perkType)
        {
            switch (perkType)
            {
                case PerkType.AutoTap:
                    return AutoTap;
                
                case PerkType.EnergyLimit:
                    return EnergyLimit;
                
                case PerkType.MultiTap:
                    return MultiTap;
            }

            return null;
        }
    }
}