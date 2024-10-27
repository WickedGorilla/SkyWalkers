using Infrastructure.Data.Game.Shop;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.RequestHandler;
using Infrastructure.Network.Response.Player;

namespace Game.Perks
{
    public class PerksService : IRequestHandler<GameData>, IRequestHandler<ValidationPaymentResponse>
    {
        public PerkEntity EnergyLimit { get; private set; }
        public PerkEntity MultiTap { get; private set; }
        public PerkEntity AutoTap { get; private set; }

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

        public void Handle(GameData response)
        {
            var perksInfo = response.PerksInfo;
            EnergyLimit = new PerkEntity(perksInfo.EnergyLimit, PerkType.EnergyLimit);
            MultiTap = new PerkEntity(perksInfo.MultiTap, PerkType.MultiTap);
            AutoTap = new PerkEntity(perksInfo.AutoTap, PerkType.AutoTap);
        }

        public void Handle(ValidationPaymentResponse response)
        {
            if (!response.IsUpdated)
                return;

            var perksInfo = response.PerksInfo;
            EnergyLimit = new PerkEntity(perksInfo.EnergyLimit, PerkType.EnergyLimit);
            MultiTap = new PerkEntity(perksInfo.MultiTap, PerkType.MultiTap);
            AutoTap = new PerkEntity(perksInfo.AutoTap, PerkType.AutoTap);
        }
    }
}