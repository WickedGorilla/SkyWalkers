using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Data.Game.Shop
{
    [CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 0)]
    public class ShopData : ScriptableObject
    {
        [SerializeField] private KeyAndValue<ItemType, ItemData>[] _items;
        [SerializeField] private KeyAndValue<PerkType, PerkData>[] _perks;

        public Dictionary<ItemType, ItemData> CreateItemsDictionary() 
            => _items.ToDictionary(x => x.Key, x => x.Value);

        public Dictionary<PerkType, PerkData> CreatePerksDictionary() 
            => _perks.ToDictionary(x => x.Key, x => x.Value);
    }
}