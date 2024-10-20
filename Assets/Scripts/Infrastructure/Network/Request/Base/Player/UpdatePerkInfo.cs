using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class UpdatePerkInfo
    {
        public int CurrentValue { get; private set; }
        public int NextValue { get; private set; }
        public int CurrentLevel { get; private set; }
        public int NextLevelPrice { get; private set; }
        public bool IsDonat { get; private set; }
    }
}