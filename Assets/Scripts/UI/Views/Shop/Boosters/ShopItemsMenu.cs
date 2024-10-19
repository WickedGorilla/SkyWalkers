using System;
using System.Collections.Generic;
using Game.Items;
using Infrastructure.Data.Game.Shop;
using SkyExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Shop.Boosters
{
    public class ShopItemsMenu : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _tittleText;
        [SerializeField] private TMP_Text _descriptionText;

        [SerializeField] private RectTransform _content; 
        [SerializeField] private ItemCard _itemCardPrefab;

        private LinkedList<ItemCard> _instantiatedItems = new();
 
        public void Open(ItemData itemData, ItemType itemType, Action<ItemEntity> onClickItem)
        {
            _iconImage.sprite = itemData.Icon;
            _tittleText.text = itemData.Name;
            _descriptionText.text = itemData.Description;

            foreach (var item in itemData.InShopVariables)
            {
                var instance = Instantiate(_itemCardPrefab, _content);
                _instantiatedItems.AddLast(instance);
                instance.SetInfo(itemData.Icon, item, () => onClickItem(new ItemEntity(itemType, item)));
            }
            
            gameObject.SetActive(true);
        }

        private void OnEnable() 
            => _backButton.onClick.AddListener(Hide);

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Hide);
            
            foreach (var item in _instantiatedItems)
                Destroy(item.gameObject);
            
            _instantiatedItems.Clear();
        }

        public void Hide() 
            => gameObject.SetActive(false);
    }
}