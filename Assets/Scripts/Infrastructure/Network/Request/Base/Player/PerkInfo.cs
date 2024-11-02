using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class PerksResponse
    {
        public PerkInfo[] Perks;
    }

    [Serializable]
    public class PerkInfo
    {
        public int Id;
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