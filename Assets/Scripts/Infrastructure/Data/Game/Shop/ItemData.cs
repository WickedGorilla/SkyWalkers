using System;
using UnityEngine;

namespace Infrastructure.Data.Game.Shop
{
    [Serializable]
    public class ItemData
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _tittle;
        [SerializeField] private string _name;
        [SerializeField] private string _description;

        public string Tittle => _tittle;
        public string Description => _description;
        public Sprite Icon => _icon;
        public string Name => _name;
    }
}