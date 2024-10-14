using System;
using UnityEngine;

namespace Infrastructure.Data
{
    [Serializable]
    public struct KeyAndValue<TKey, TValue>
    {
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public TKey Key => _key;
        public TValue Value => _value;
    }
}