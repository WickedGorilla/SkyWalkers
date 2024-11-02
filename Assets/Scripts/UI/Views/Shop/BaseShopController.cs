using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Items;
using Game.Perks;
using Infrastructure;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network;
using Infrastructure.Network.Request;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.Response;
using Player;
using SkyExtensions;
using UI.Core;
using UnityEngine;

namespace UI.Views
{
    public abstract class BaseShopController<TShopView> : ViewController<TShopView> where TShopView : ShopViewBase
    {
        private readonly IServerRequestSender _serverRequestSender;
        private readonly PerksService _perksService;
        private readonly WalletService _walletService;
        private readonly OnGameFocusEvent _onGameFocusEvent;

        private readonly Dictionary<PerkType, PerkData> _perksData;
        private readonly Dictionary<ItemType, ItemData> _itemData;

        private readonly LinkedList<IDisposable> _disposables = new();

        protected BaseShopController(TShopView view,
            IServerRequestSender serverRequestSender,
            PerksService perksService,
            ShopData shopData,
            WalletService walletService,
            OnGameFocusEvent onGameFocusEvent) : base(view)
        {
            _serverRequestSender = serverRequestSender;
            _perksService = perksService;
            _walletService = walletService;
            _onGameFocusEvent = onGameFocusEvent;

            _itemData = shopData.CreateItemsDictionary();
            _perksData = shopData.CreatePerksDictionary();
        }

        protected override void OnShow()
        {
            View.ShopButton.onClick.AddListener(OnClickShopButton);
            View.BoostersButton.onClick.AddListener(OnClickBoostersButton);

            foreach (var boostersCard in View.BoostersCards)
            {
                _disposables.AddLast(boostersCard.ClickButton.AddListener(OnClickCard));

                void OnClickCard()
                    => OpenItemCard(boostersCard.Type);
            }

            foreach (var upgrade in View.Upgrades)
            {
                _disposables.AddLast(upgrade.ClickButton.AddListener(OnClickUpgrade));

                void OnClickUpgrade()
                    => OpenUpgradeCard(upgrade.Type);
            }
        }

        protected override void OnHide()
        {
            View.ShopButton.onClick.RemoveListener(OnClickShopButton);
            View.BoostersButton.onClick.RemoveListener(OnClickBoostersButton);

            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void OpenUpgradeCard(PerkType perkType)
        {
            var perk = _perksService.GetPerkByType(perkType);

            if (perk.CurrentLevel > 0)
            {
                View.UpgradesPerkMenu.Open(_perksData[perkType], _walletService.Coins, perk, 
                    () => OnClickBuyUpgrade(perk));
            }
            else
            {
                View.BuyItemMenu.Open(_perksData[perkType], _walletService.Coins, perk, 
                    () => OnClickBuyUpgrade(perk));
            }
        }

        private void OpenItemCard(ItemType itemType)
            => View.ItemsMenu.Open(_itemData[itemType], itemType, OnClickItem);

        private void OnClickItem(ItemEntity itemEntity)
            => View.BuyItemMenu.Open(_itemData[itemEntity.Type], _walletService.Coins, itemEntity, () => OnClickBuyItem(itemEntity));

        private async void OnClickBuyItem(ItemEntity itemEntity)
        {
            View.ShowLoader();

            var request = new PaymentLinkItemRequest((int)itemEntity.Type, itemEntity.Amount);
            var response = await _serverRequestSender.SendToServer<PaymentLinkItemRequest, PaymentItemResult>(request,
                ServerPath.PaymentItem);
            
            if (!response.Success)
            {
                View.HideLoader();
                return;
            }

            var data = response.Data;
            
            if (string.IsNullOrEmpty(data.PaymentUrl))
            {
                View.HideLoader();
                return;
            }
            
            _onGameFocusEvent.AddOnFocusEvent(() => SendValidationPayment(data.OrderCode));
            Application.OpenURL(data.PaymentUrl);
        }

        private async void OnClickBuyUpgrade(PerkEntity perkEntity)
        {
            View.ShowLoader();

            var request = new PaymentLinkPerkUpgradeRequest((int)perkEntity.PerkType, perkEntity.NextLevel);
            var response = await _serverRequestSender.SendToServerAndHandle<PaymentLinkPerkUpgradeRequest, 
                PaymentUpgradePerkResult>(request, ServerPath.PaymentPerk);

            if (!response.Success)
            {
                View.HideLoader();
                return;
            }

            var data = response.Data;
            
            if (string.IsNullOrEmpty(data.PaymentUrl))
            {
                View.HideLoader();
                return;
            }
            
            _onGameFocusEvent.AddOnFocusEvent(() => SendValidationPayment(data.OrderCode));
            Application.OpenURL(data.PaymentUrl);
        }

        private void OnClickShopButton()
            => View.ShowShopMenu();

        private void OnClickBoostersButton()
            => View.ShowBoosters();

        private async void SendValidationPayment(string orderCode)
        {
            await UniTask.WaitForSeconds(1.7f);
            
            await _serverRequestSender.SendToServerAndHandle<ValidationPaymentRequest, ValidationPaymentResponse>(
                new ValidationPaymentRequest(orderCode),
                ServerPath.PaymentValidation);
            
            View.HideLoader();
        }
    }
}