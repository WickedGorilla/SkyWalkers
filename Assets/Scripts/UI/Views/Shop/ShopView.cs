using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ShopView : View
    {
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _boostersButton;
        [SerializeField] private BoosterContainer[] _boostersCards;
        
        public Button ShopButton => _shopButton;
        public Button BoostersButton => _boostersButton;
        public BoosterContainer[] BoostersCards => _boostersCards;

        [Serializable]
        public class BoosterContainer
        {
            [SerializeField] private Button _clickButton;
            [SerializeField] private BoostersType _type;
        }
        
        public enum BoostersType
        {
            EnergyLimit = 0,
            Boost = 1,
            MultiTap = 2,
            PlayPass = 3,
        }
    }
}