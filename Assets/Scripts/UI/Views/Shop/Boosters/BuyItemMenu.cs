using System;
using Game.Items;
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
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Button _buyButton;
        
        private IDisposable _disposable;

        public void Open(PerkData perkData, PerkEntity perk)
        {
            _iconImage.sprite = perkData.Icon;
            _titleText.text = perkData.Tittle;
            _descriptionText.text = perkData.UpgradeDescriptionText;

            SetPrice(perk.NextLevelPrice, perk.IsDonat);
        }

        public void Open(ItemData itemData, ItemEntity item)
        {
            _iconImage.sprite = itemData.Icon;
            _titleText.text = itemData.Tittle;
            _descriptionText.text = itemData.Description;
            
            SetPrice(item.Price, item.IsDonat);
        }

        private void SetPrice(int price, bool isDonat)
        {
            string currency = SpritesAtlasCode.GetCurrentCurrencyCode(isDonat);
            _priceText.text = $"{currency} {price}";
            _buttonText.text = $"{currency} buy now";
        }
        
        private void OnEnable() 
            => _disposable = _closeButton.AddListener(() => gameObject.SetActive(false));

        private void OnDisable()
            => _disposable.Dispose();
        

    }
}