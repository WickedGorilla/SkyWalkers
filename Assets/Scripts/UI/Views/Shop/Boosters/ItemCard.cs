using System;
using Infrastructure.Data.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Views.Shop.Boosters
{
    public class ItemCard : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _clickButton;
        
        public void SetInfo(Sprite icon, ItemData.ShopVariable item, UnityAction onClick)
        {
            _image.sprite = icon;
            _nameText.text = item.Tittle;
            _descriptionText.text = item.Description;
            _priceText.text = $"{SpritesAtlasCode.GetCurrentCurrencyCode(item.IsDonat)} {item.Price}";
            _clickButton.onClick.AddListener(onClick);
        }

        private void OnDisable() 
            => _clickButton.onClick.RemoveAllListeners();
    }
}