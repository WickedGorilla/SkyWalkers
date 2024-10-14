using System;
using Game.Perks;
using Infrastructure.Data.Game.Shop;
using SkyExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Shop.Boosters
{
    public class BuyItemMenu : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _rocketImage;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _buttonText;
        
        private IDisposable _disposable;

        public void Open(PerkData perkData, PerkEntity perk)
        {
            
        }

        private void OnEnable() 
            => _disposable = _closeButton.AddListener(() => gameObject.SetActive(false));

        private void OnDisable() 
            => _disposable.Dispose();
    }
}