using Infrastructure.Network.Response.Player;

namespace Game.Perks
{
    public class PerkEntity
    {
        public PerkEntity(PerkInfo info)
        {
            CurrentValue = info.CurrentValue;
            CurrentLevel = info.CurrentLevel;
            NextValue = info.NextValue;
            MaxLevel = info.MaxLevel;
            NextLevelPrice = info.NextLevelPrice;
            IsDonat = info.IsDonat;
        }

        public int CurrentValue { get; private set; }
        public int NextValue { get; private set; }
        public int CurrentLevel { get; private set; }
        public int MaxLevel { get; private set; }
        public int NextLevelPrice { get; private set; }
        public bool IsDonat { get; private set; }

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