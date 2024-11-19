using System;
using Game.Items;
using Game.Perks;
using Infrastructure.Data.Game.Shop;
using SkyExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

        public event Action<bool> OnShow;

        public void Open(ItemData itemData, int coins, ItemEntity item, UnityAction buyAction)
        {
            gameObject.SetActive(true);
            _iconImage.sprite = itemData.Icon;
            _titleText.text = itemData.Tittle;
            _descriptionText.text = itemData.Description;

            SetPrice(item.Price, coins, item.IsDonat);
            _buyButton.onClick.AddListener(buyAction);
        }

        public void Open(PerkData data, int coins, PerkEntity perk, UnityAction buyAction)
        {
            gameObject.SetActive(true);
            _iconImage.sprite = data.Icon;
            _titleText.text = $"{perk.NextLevel} {UpgradePerkMenu.LevelText} {data.Tittle}" ;
            _descriptionText.text =  $"{data.UpgradeDescriptionText} {perk.NextValue.ToString().ApplyPurpleColor()} {data.UpgradeDescriptionTextAfterValue}";
            
            SetPrice(perk.NextLevelPrice, coins, perk.IsDonat);
            _buyButton.onClick.AddListener(buyAction);
        }
        
        private void SetPrice(int price, int myCoins, bool isDonat)
        {
            var isLock = !isDonat && price > myCoins;
            _buyButton.interactable = !isLock;
            
            string currency = SpritesAtlasCode.GetCurrentCurrencyCode(isDonat);
            _priceText.text = $"{currency} {NumbersFormatter.GetCountVariant(price)}";
            
            var buttonText = isLock ? $"{SpritesAtlasCode.Lock}" : $"{currency}";
            _buttonText.text = $"{buttonText} buy now";
        }

        private void OnEnable()
        {
            _disposable = _closeButton.SubscribeListener(() => gameObject.SetActive(false));
            OnShow?.Invoke(true);
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveAllListeners();
            _disposable.Dispose();
            OnShow?.Invoke(false);
        }
    }
}