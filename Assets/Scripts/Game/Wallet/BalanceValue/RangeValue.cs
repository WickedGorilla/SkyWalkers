using System;

namespace Game.Wallet.Flash
{
    [Serializable]
    public struct RangeValue
    {
        public int CurrentCount;
        public int MaxCount;

        public RangeValue(int currentCount, int maxCount)
        {
            CurrentCount = currentCount;
            MaxCount = maxCount;
        }
    }
}