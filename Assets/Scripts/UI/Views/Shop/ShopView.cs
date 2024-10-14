using System;
using Infrastructure.Data.Game.Shop;
using UI.Core;
using UI.Views.Shop.Boosters;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace UI.Views
{
    public class ShopView : View
    {
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _boostersButton;
        [SerializeField] private ScrollRect _boostersScrollView;
        [SerializeField] private GameObject _shopView;
        [SerializeField] private BoosterContainer[] _boosters;
        [SerializeField] private UpgradeContainer[] _upgrades;
        [SerializeField] private UpgradePerkMenu _upgradesPerkMenu;
        [SerializeField] private BuyItemMenu _buyItemMenu;
        [SerializeField] private ShopItemsMenu _itemsMenu;

        public Button ShopButton => _shopButton;
        public Button BoostersButton => _boostersButton;
        public BoosterContainer[] BoostersCards => _boosters;
        public UpgradeContainer[] Upgrades => _upgrades;
        public UpgradePerkMenu UpgradesPerkMenu => _upgradesPerkMenu;
        public BuyItemMenu BuyItemMenu => _buyItemMenu;
        public ShopItemsMenu ItemsMenu => _itemsMenu;

        public void ShowBoosters()
        {
            _shopView.SetActive(false);
            _boostersScrollView.gameObject.SetActive(true);
        }

        public void ShowShopMenu()
        {
            _shopView.SetActive(true);
            _boostersScrollView.gameObject.SetActive(false);
        }

        [Serializable]
        public class BoosterContainer
        {
            [SerializeField] private Button _clickButton;
            [SerializeField] private ItemType _type;

            public Button ClickButton => _clickButton;
            public ItemType Type => _type;
        }

        [Serializable]
        public class UpgradeContainer
        {
            [SerializeField] private Button _clickButton;
            [SerializeField] private PerkType _type;

            public Button ClickButton => _clickButton;
            public PerkType Type => _type;
        }
    }
}