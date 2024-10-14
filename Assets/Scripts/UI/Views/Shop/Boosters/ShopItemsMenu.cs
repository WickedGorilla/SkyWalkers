using System;
using System.Collections.Generic;
using Game.Items;
using Infrastructure.Data.Game.Shop;
using SkyExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("_itemViewPrefab")] [SerializeField] private ItemCard itemCardPrefab;

        private LinkedList<ItemCard> _instantiatedItems = new();
        private IDisposable _disposable;

        public void Open(ItemData itemData, ItemEntity[] itemsEntity)
        {
            _iconImage.sprite = itemData.Icon;
            _tittleText.text = itemData.Name;
            _descriptionText.text = itemData.Description;

            foreach (var item in itemsEntity)
            {
                var instance = Instantiate(itemCardPrefab);
                _instantiatedItems.AddLast(instance);
                instance.SetInfo(itemData, item.Amount, item.Price, item.isDonat);
            }
            
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
           _disposable = _backButton.AddListener(() => gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            _disposable.Dispose();
            
            foreach (var item in _instantiatedItems)
                Destroy(item.gameObject);
        }
    }
}