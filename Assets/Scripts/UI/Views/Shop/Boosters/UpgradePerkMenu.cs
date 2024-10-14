using Game.Perks;
using Infrastructure.Data.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Shop.Boosters
{
    public class UpgradePerkMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsCountText;
        
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _iconText;
        [SerializeField] private TMP_Text _tittleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _upgradeDescriptionText;
        [SerializeField] private TMP_Text _levelText;

        public void SetCountText(int count)
        {
            _coinsCountText.text = count.ToString();
        }

        public void Open(PerkData data, PerkEntity perkEntity, int countCoins)
        {
            _iconImage.sprite = data.Icon;
            _iconText.text = data.GetIconText(perkEntity.CurrentValue);
            _tittleText.text = data.Tittle;
            _descriptionText.text = data.Description;
            _upgradeDescriptionText.text = data.UpgradeDescriptionText;
            _levelText.text = $"{perkEntity.MaxLevel} • {GetCurrentCurrencyCode(perkEntity.IsDonat)} {perkEntity.NextLevelPrice}";

            SetCountText(countCoins);
        }

        private string GetCurrentCurrencyCode(bool isDonat) 
            => isDonat ? SpritesAtlasCode.Star : SpritesAtlasCode.Coins;
    }
}