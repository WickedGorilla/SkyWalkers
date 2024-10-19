using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class PerksInfo
    {
        public PerkInfo EnergyLimit;
        public PerkInfo MultiTap;
        public PerkInfo AutoTap;

        public PerksInfo()
        {
            EnergyLimit = new PerkInfo(100, 200, 1, 10, 300, true);
            MultiTap = new PerkInfo(100, 200, 1, 10, 300, true);
            AutoTap = new PerkInfo(100, 200, 1, 10, 300, true);
        }
    }

    [Serializable]
    public class PerkInfo
    {
        public int CurrentValue;
        public int NextValue;
        public int CurrentLevel;
        public int MaxLevel;
        public int NextLevelPrice;
        public bool IsDonat;

        public PerkInfo(int currentValue, int nextValue, int currentLevel, int maxLevel, int nextLevelPrice, bool isDonat)
        {
            CurrentValue = currentValue;
            NextValue = nextValue;
            CurrentLevel = currentLevel;
            MaxLevel = maxLevel;
            NextLevelPrice = nextLevelPrice;
            IsDonat = isDonat;
        }
    }
}