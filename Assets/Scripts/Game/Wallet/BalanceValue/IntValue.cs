using System;
using UnityEngine;

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

            if (Count < 0)
            {
                Count = 0;
                Debug.LogError("Error balance Value");
            }
            
            OnChangeValue?.Invoke(Count);
            return Count;
        }

        public void Update(int value)
        {
            Count = value;
            OnChangeValue?.Invoke(Count);
        }

        public static implicit operator IntValue(int value) => new(value);
        public static implicit operator int(IntValue addableInt) => addableInt.Count;

        public bool Subtract(int count)
        {
            if (count > Count)
                return false;

            Add(-count);
            return true;
        }
    }
}