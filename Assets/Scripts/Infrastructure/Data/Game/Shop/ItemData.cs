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
        
        [SerializeField] private ShopVariable[] _shopVariables;
        
        public string Tittle => _tittle;
        public string Description => _description;
        public Sprite Icon => _icon;
        public string Name => _name;
        public ShopVariable[] InShopVariables => _shopVariables;

        [Serializable]
        public class ShopVariable
        {
            [SerializeField] private string _tittle;
            [SerializeField] private string _description;
            [SerializeField] private int _price;
            [SerializeField] private int _amount;
            [SerializeField] private bool _isDonat;

            public string Tittle => _tittle;
            public string Description => _description;
            public int Price => _price;
            public bool IsDonat => _isDonat;

            public int Amount => _amount;
        }
    }
}