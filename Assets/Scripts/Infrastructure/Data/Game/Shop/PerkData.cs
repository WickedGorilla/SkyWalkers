using System;
using UnityEngine;

namespace Infrastructure.Data.Game.Shop
{
    [Serializable]
    public class PerkData
    {
        [SerializeField] private string _tittle;
        [SerializeField] private string _description;

        public string Tittle => _tittle;
        public string Description => _description;
    }
}