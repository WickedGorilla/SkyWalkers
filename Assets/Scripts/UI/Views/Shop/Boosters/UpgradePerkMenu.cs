using Game.Perks;
using Infrastructure.Data.Game.Shop;
using TMPro;
using UnityEngine;

namespace UI.Views.Shop.Boosters
{
    public class UpgradePerkMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsCountText;
        [SerializeField] private TMP_Text _iconText;
        [SerializeField] private TMP_Text _tittleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _upgradeDescriptionText;
        [SerializeField] private TMP_Text _levelText;

        public void SetCountText(int count)
        {
            _coinsCountText.text = count.ToString();
        }

        public void Open(PerkData data, PerkEntity perkEntity)
        {
           
        }
    }
}