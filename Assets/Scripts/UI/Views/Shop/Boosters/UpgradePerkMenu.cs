using System;
using Game.Perks;
using Infrastructure.Data.Game.Shop;
using SkyExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Views.Shop.Boosters
{
    public class UpgradePerkMenu : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Text _coinsCountText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _iconText;
        [SerializeField] private TMP_Text _tittleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _upgradeDescriptionText;
        [SerializeField] private TMP_Text _levelText;

        [Header("Upgrade Button")]
        [SerializeField] private Button _upgradeButton;

        [SerializeField] private TMP_Text _upgradeButtonText;
        [SerializeField] private string _upgradeText = "upgrade now";

        private IDisposable _disposable;

        public event Action<bool> OnShow;

        private void OnEnable()
        {
            _disposable = _backButton.SubscribeListener(() => { gameObject.SetActive(false); });
            OnShow?.Invoke(true);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveAllListeners();
            _disposable?.Dispose();
            OnShow?.Invoke(false);
        }

        public void SetCountText(int count)
        {
            _coinsCountText.text = $"{SpritesAtlasCode.Coin} {count.ToString()}";
        }

        public void Open(PerkData data,  int countCoins, PerkEntity perkEntity, UnityAction onUpgradeButton)
        {
            gameObject.gameObject.SetActive(true);
            _upgradeButton.onClick.AddListener(onUpgradeButton);
            
            _iconImage.sprite = data.Icon;
            _iconText.text = data.GetIconText(perkEntity.CurrentValue);
            _tittleText.text = data.Tittle;
            _descriptionText.text = data.Description;
            _upgradeDescriptionText.text = data.UpgradeDescriptionText;
            _levelText.text =
                $"{perkEntity.NextLevel} LVL • {GetCurrentCurrencyCode(perkEntity.IsDonat)} {perkEntity.NextLevelPrice}";
            
            if (perkEntity.CurrentLevel == perkEntity.MaxLevel || !CheckBalance())
                LockButton();
            else   
                UnlockButton();
            
            SetCountText(countCoins);

            bool CheckBalance()
                => perkEntity.IsDonat || perkEntity.NextLevelPrice <= countCoins;
        }

        private void LockButton()
        {
            _upgradeButtonText.text = $"{SpritesAtlasCode.Lock} {_upgradeText}";
            _upgradeButton.interactable = false;
        }

        private void UnlockButton()
        {
            _upgradeButtonText.text = $"{SpritesAtlasCode.Magic} {_upgradeText}";
            _upgradeButton.interactable = true;
        }

        private string GetCurrentCurrencyCode(bool isDonat)
            => isDonat ? SpritesAtlasCode.Star : SpritesAtlasCode.Coin;
    }
}