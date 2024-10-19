using Infrastructure.Data.Game.Shop;
using Infrastructure.Network.Response.Player;

namespace Game.Perks
{
    public class PerkEntity
    {

        public PerkEntity(PerkInfo info, PerkType perkType)
        {
            CurrentValue = info.CurrentValue;
            CurrentLevel = info.CurrentLevel;
            NextValue = info.NextValue;
            MaxLevel = info.MaxLevel;
            NextLevelPrice = info.NextLevelPrice;
            IsDonat = info.IsDonat;
            PerkType = perkType;
        }

        public int CurrentValue { get; private set; }
        public int NextValue { get; private set; }
        public int CurrentLevel { get; private set; }

        public int MaxLevel { get; private set; }
        public int NextLevelPrice { get; private set; }
        public bool IsDonat { get; private set; }

        public int NextLevel
            => CurrentLevel == MaxLevel ? MaxLevel : CurrentLevel++;

        public PerkType PerkType { get; set; }

        public void Upgrade(UpdatePerkInfo info)
        {
            CurrentValue = info.CurrentValue;
            NextValue = info.NextValue;
            CurrentLevel = info.CurrentLevel;
            NextLevelPrice = info.NextLevelPrice;
            IsDonat = info.IsDonat;
        }
    }
}