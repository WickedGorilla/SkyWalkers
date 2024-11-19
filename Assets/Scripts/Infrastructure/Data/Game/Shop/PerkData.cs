using System;
using UnityEngine;

namespace Infrastructure.Data.Game.Shop
{
    [Serializable]
    public class PerkData
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _prevIconText;
        [SerializeField] private string _lastIconText;
        [SerializeField] private string _tittle;
        [SerializeField] private string _description;
        [SerializeField] private string _upgradeDescriptionText;
        [SerializeField] private string _upgradeDescriptionTextAfterValue;

        public string Tittle => _tittle;
        public string Description => _description;
        public Sprite Icon => _icon;
        public string UpgradeDescriptionText => _upgradeDescriptionText;
        public string UpgradeDescriptionTextAfterValue => _upgradeDescriptionTextAfterValue;

        public string GetIconText(int currentValue) 
            => $"{_prevIconText}{currentValue}{_lastIconText}";
    }
}