using System;
using UnityEngine;

namespace Infrastructure.Data.Game.Shop
{
    [Serializable]
    public class PerkData
    {
        [SerializeField] private string _tittle;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;

        public string Tittle => _tittle;
        public string Description => _description;
        public Sprite Icon => _icon;
    }
}