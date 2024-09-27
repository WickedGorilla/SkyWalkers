using System;
using Game.Wallet.Flash;
using UnityEngine;

namespace Game.Wallet
{
    public class IntRangeValue : IBalanceValue<int>
    {
        private RangeValue _value;
        
        public IntRangeValue(int current, int max) 
            => _value = new RangeValue(current, max);

        public IntRangeValue(RangeValue value) 
            => _value = value;

        public event Action<int> OnChangeValue;
            
        public int Count 
        { 
            get => _value.CurrentCount;
            private set => _value.CurrentCount = value; 
        }

        public int Max => _value.MaxCount;
        
        public int Add(int value)
        {
            Count += value;

            if (Count < 0 || Count > _value.MaxCount)
            {
                Count = 0;
                Debug.LogError("Error balance Value");
            }
            
            OnChangeValue?.Invoke(Count);
            
            return Count;
        }
        
        public static implicit operator RangeValue(IntRangeValue addableInt) => addableInt._value;
    }
}