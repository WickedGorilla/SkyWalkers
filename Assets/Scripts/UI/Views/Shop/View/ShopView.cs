using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.Views
{
    public class ShopView : ShopViewBase
    {
        private readonly Dictionary<Type, bool> _openedItemsMenu = new();

        private bool _currentState;
        public event Action<bool> OnAnyItemMenuOpened;

        public override void OnShow()
        {
            base.OnShow();

            UpgradesPerkMenu.OnShow += OnShowPerkMenu;
            BuyItemMenu.OnShow += OnShowBuyItemMenu;
            ItemsMenu.OnShow += OnShowItemsMenu;
        }

        public override void OnHide()
        {
            UpgradesPerkMenu.OnShow -= OnShowPerkMenu;
            BuyItemMenu.OnShow -= OnShowBuyItemMenu;
            ItemsMenu.OnShow -= OnShowItemsMenu;

            base.OnHide();
        }

        private void OnShowPerkMenu(bool value)
        {
            _openedItemsMenu[UpgradesPerkMenu.GetType()] = value;
            UpdateState();
        }

        private void OnShowBuyItemMenu(bool value)
        {
            _openedItemsMenu[BuyItemMenu.GetType()] = value;
            UpdateState();
        }

        private void OnShowItemsMenu(bool value)
        {
            _openedItemsMenu[ItemsMenu.GetType()] = value;
            UpdateState();
        }

        private void UpdateState()
        {
            bool value = _openedItemsMenu.Any(kvp => kvp.Value);

            if (_currentState == value)
                return;

            _currentState = value;
            OnAnyItemMenuOpened?.Invoke(_currentState);
        }
    }
}