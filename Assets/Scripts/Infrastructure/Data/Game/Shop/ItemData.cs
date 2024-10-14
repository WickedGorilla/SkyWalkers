using System;
using UnityEngine;

namespace Infrastructure.Data.Game.Shop
{
    [Serializable]
    public class ItemData
    {
        [SerializeField] private string _tittle;
        [SerializeField] private string _description;
        [SerializeField] private int _price;
        [SerializeField] private bool _isDonat;

        public string Tittle => _tittle;
        public string Description => _description;
        public int Price => _price;
        public bool IsDonat => _isDonat;
    }
}