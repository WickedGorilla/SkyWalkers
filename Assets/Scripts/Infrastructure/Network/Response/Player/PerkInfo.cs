using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class PerksInfo
    {
        public PerkInfo EnergyLimit;
        public PerkInfo MultiTap;
        public PerkInfo AutoTap;
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
    }
}