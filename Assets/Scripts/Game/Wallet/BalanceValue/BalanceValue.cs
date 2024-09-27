using System;

namespace Game.Wallet
{
    public class BalanceValue<TValue> where TValue : IBalanceValue<TValue>
    {
        public TValue Value { get; private set; }

        public event Action<TValue> OnValueChanged;

        public void Initialize(TValue value)
        {
            Value = value;
        }

        public void Collect(TValue amount)
        {
            Value.Add(amount);
            OnValueChanged?.Invoke(Value);
        }
    }
}