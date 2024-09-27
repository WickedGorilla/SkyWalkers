using System;

namespace Game.Wallet
{
    public class IntValue : IBalanceValue<int>
    {
        public int Count { get; private set; }

        public event Action<int> OnChangeValue;

        public IntValue(int count = default)
        {
            Count = count;
        }

        public int Add(int value)
        {
            Count += value;
            OnChangeValue?.Invoke(Count);

            return Count;
        }

        public static implicit operator IntValue(int value) => new(value);
        public static implicit operator int(IntValue addableInt) => addableInt.Count;
    }
}