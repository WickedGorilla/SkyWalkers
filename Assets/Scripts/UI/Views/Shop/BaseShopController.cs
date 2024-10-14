using System;
using System.Collections.Generic;
using Game.Perks;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network;
using Infrastructure.Network.Request;
using Infrastructure.Network.Response;
using Player;
using SkyExtensions;
using UI.Core;

namespace UI.Views
{
    public abstract class BaseShopController<TShopView> : ViewController<TShopView> where TShopView : ShopView
    {
        private readonly IServerRequestSender _serverRequestSender;
        private readonly PerksService _perksService;
        private readonly WalletService _walletService;

        private readonly Dictionary<PerkType, PerkData> _perksData;
        private readonly Dictionary<ItemType, ItemData> _itemData;
        
        private readonly LinkedList<IDisposable> _disposables = new();

        protected BaseShopController(TShopView view,
            IServerRequestSender serverRequestSender, 
            PerksService perksService, 
            ShopData shopData,
            WalletService walletService) : base(view)
        {
            _serverRequestSender = serverRequestSender;
            _perksService = perksService;
            _walletService = walletService;

            _itemData = shopData.CreateItemsDictionary();
            _perksData = shopData.CreatePerksDictionary();
        }
        
        protected override void OnShow()
        {
            View.ShopButton.onClick.AddListener(OnClickShopButton);
            View.BoostersButton.onClick.AddListener(OnClickBoostersButton);

            foreach (var boostersCard in View.BoostersCards)
            {
                _disposables.AddLast(boostersCard.ClickButton.AddListener(OnClickItem));

                void OnClickItem() 
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
                View.UpgradesPerkMenu.Open(_perksData[perkType], perk, _walletService.Coins);
            else
                View.BuyItemMenu.Open(_perksData[perkType], perk);
        }
        
        private void OpenItemCard(ItemType itemType)
        {
            
        }
        
        private void ShowLoader()
        {
            _serverRequestSender.SendToServer<PaymentLinkRequest, PaymentLinkResponse>(new PaymentLinkRequest(0, 0, 1),
                ServerPath.Payment, OnComplete);

            void OnComplete(ServerResponse<PaymentLinkResponse> response)
                => HideLoader();

            void OnError(string error)
                => HideLoader();
        }

        private void HideLoader()
        {
            //View.ShowLoader(false);
        }

        private void OnClickShopButton()
            => View.ShowShopMenu();

        private void OnClickBoostersButton()
            => View.ShowBoosters();
    }

    public class ShopController : BaseShopController<ShopView>
    {

        public ShopController(ShopView view,
            IServerRequestSender serverRequestSender, 
            PerksService perksService, 
            ShopData data, 
            WalletService walletService) 
            : base(view, serverRequestSender, perksService, data, walletService)
        {
        }
    }
}