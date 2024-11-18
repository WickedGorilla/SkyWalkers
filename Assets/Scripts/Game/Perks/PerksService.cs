using System;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network.Request.Base.Player;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.RequestHandler;
using Infrastructure.Network.Response.Player;
using UnityEngine;

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
                case PerkType.EnergyLimit:
                    return EnergyLimit;
                
                case PerkType.AutoTap:
                    return AutoTap;

                case PerkType.MultiTap:
                    return MultiTap;
            }

            return null;
        }

        private void SetPerkByType(PerkType perkType, PerkInfo perkInfo)
        {
            switch (perkType)
            {
                case PerkType.EnergyLimit:
                    EnergyLimit = new PerkEntity(perkInfo, perkType);
                    return;
                
                case PerkType.AutoTap:
                    AutoTap = new PerkEntity(perkInfo, perkType);
                    return;

                case PerkType.MultiTap:
                    MultiTap = new PerkEntity(perkInfo, perkType);
                    return;
            }

        }
        
        public void Handle(GameData response)
        {
            if (response.Perks.Perks.Length == 0)
                return;

            HandlePerk(response.Perks);
        }

        public void Handle(ValidationPaymentResponse response) 
            => HandlePerk(response.Perks);

        private void HandlePerk(PerksResponse perks)
        {
            foreach (PerkInfo perkInfo in perks.Perks)
            {
                if (!Enum.IsDefined(typeof(PerkType), perkInfo.Id))
                {
                    Debug.LogError("The integer does not correspond to a defined enum value.");
                    return;
                }
                
                PerkType enumValue = (PerkType)perkInfo.Id;
                SetPerkByType(enumValue, perkInfo);
            }
        }
    }
}