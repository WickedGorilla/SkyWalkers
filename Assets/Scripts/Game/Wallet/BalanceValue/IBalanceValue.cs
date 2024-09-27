using System;

namespace Game.Wallet
{
    public interface IBalanceValue<T>
    {
        event Action<T> OnChangeValue;
        
        T Add(T value);
    }
}