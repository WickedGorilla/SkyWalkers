using System;
using System.Collections.Generic;
using Game.Items;
using Game.Perks;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network;
using Infrastructure.Network.Request;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.Response;
using Infrastructure.Telegram;
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
        private readonly TelegramLauncher _telegramLauncher;

        private readonly Dictionary<PerkType, PerkData> _perksData;
        private readonly Dictionary<ItemType, ItemData> _itemData;

        private readonly LinkedList<IDisposable> _disposables = new();

        protected BaseShopController(TShopView view,
            IServerRequestSender serverRequestSender,
            PerksService perksService,
            ShopData shopData,
            WalletService walletService,
            TelegramLauncher telegramLauncher) : base(view)
        {
            _serverRequestSender = serverRequestSender;
            _perksService = perksService;
            _walletService = walletService;
            _telegramLauncher = telegramLauncher;

            _itemData = shopData.CreateItemsDictionary();
            _perksData = shopData.CreatePerksDictionary();
        }

        protected override void OnShow()
        {
            View.ShopButton.onClick.AddListener(OnClickShopButton);
            View.BoostersButton.onClick.AddListener(OnClickBoostersButton);

            foreach (var boostersCard in View.BoostersCards)
            {
                _disposables.AddLast(boostersCard.ClickButton.SubscribeListener(OnClickCard));

                void OnClickCard()
                    => OpenItemCard(boostersCard.Type);
            }

            foreach (var upgrade in View.Upgrades)
            {
                _disposables.AddLast(upgrade.ClickButton.SubscribeListener(OnClickUpgrade));

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
                    () => OnClickBuyUpgrade(perk, View.UpgradesPerkMenu));
            }
            else
            {
                View.BuyItemMenu.Open(_perksData[perkType], _walletService.Coins, perk,
                    () => OnClickBuyUpgrade(perk, View.UpgradesPerkMenu));
            }
        }

        private void OpenItemCard(ItemType itemType)
            => View.ItemsMenu.Open(_itemData[itemType], itemType, OnClickItem);

        private void OnClickItem(ItemEntity itemEntity)
        {
            View.BuyItemMenu.Open(_itemData[itemEntity.Type], _walletService.Coins, itemEntity,
                () => OnClickBuyItem(itemEntity, View.BuyItemMenu));
        }

        private async void OnClickBuyItem(ItemEntity itemEntity, MonoBehaviour openedView)
        {
            View.ShowLoader();

            var request = new PayItemRequest((int)itemEntity.Type, itemEntity.Amount);
            var response = await _serverRequestSender.SendToServer<PayItemRequest, PaymentItemResult>(request,
                ServerAddress.PaymentItem);

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
            
            _telegramLauncher.OpenInvoiceLink(data.PaymentUrl, Action);
            
            async void Action()
                => await SendValidationPayment(data.OrderCode, openedView);
        }

        private async void OnClickBuyUpgrade(PerkEntity perkEntity, MonoBehaviour openedView)
        {
            View.ShowLoader();

            var request = new PayUpgradeRequest((int)perkEntity.PerkType, perkEntity.NextLevel);
            var response = await _serverRequestSender.SendToServerAndHandle<PayUpgradeRequest,
                PaymentUpgradePerkResult>(request, ServerAddress.PaymentPerk);

            if (!response.Success)
            {
                View.HideLoader();
                return;
            }

            var data = response.Data;

            if (string.IsNullOrEmpty(data.PaymentUrl))
            {
                View.HideLoader();
                openedView.gameObject.SetActive(false);
                OpenUpgradeCard(perkEntity.PerkType);
                return;
            }

            _telegramLauncher.OpenInvoiceLink(data.PaymentUrl, 
                () => SendValidationUpgrade(data.OrderCode, perkEntity.PerkType, openedView));
        }

        private async void SendValidationUpgrade(string orderCode, PerkType perkType, MonoBehaviour openedView)
        {
            await SendValidationPayment(orderCode, openedView);
            OpenUpgradeCard(perkType);
        }
        
        private void OnClickShopButton()
            => View.ShowShopMenu();

        private void OnClickBoostersButton()
            => View.ShowBoosters();

        private async Awaitable SendValidationPayment(string orderCode, MonoBehaviour openedView = null)
        {
            await Awaitable.WaitForSecondsAsync(1.7f);

            await _serverRequestSender.SendToServerAndHandle<ValidationPaymentRequest, ValidationPaymentResponse>(
                new ValidationPaymentRequest(orderCode),
                ServerAddress.PaymentValidation);

            if (openedView != null)
                openedView.gameObject.SetActive(false);
            
            View.HideLoader();
        }
    }
}