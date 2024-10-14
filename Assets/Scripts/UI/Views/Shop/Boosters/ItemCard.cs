using Infrastructure.Data.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Shop.Boosters
{
    public class ItemCard : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _priceText;

        public void SetInfo(ItemData itemData, int count, int price, bool isDonat)
        {
            _image.sprite = itemData.Icon;
            _nameText.text = $"{count} {itemData.Name}";
            _descriptionText.text = $"{itemData.Description}";
            _priceText.text = $"{SpritesAtlasCode.GetCurrentCurrencyCode(isDonat)} {price}";
        }
    }
}