using System;

namespace Player
{
    public class BalanceValue
    {
        public int Value { get; private set; }

        public event Action<int> OnValueChanged;

        public void Initialize(int value)
        {
            Value = value;
        }

        public void Collect(int amount)
        {
            Value += amount;
            OnValueChanged?.Invoke(Value);
        }

        public void Collect()
        {
            Value++;
            OnValueChanged?.Invoke(Value);
        }
        
    }
}