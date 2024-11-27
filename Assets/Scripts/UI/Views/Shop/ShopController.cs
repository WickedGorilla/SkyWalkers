using Game.Perks;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network;
using Infrastructure.Telegram;
using Player;
using UI.Core;

namespace UI.Views
{
    public class ShopController : BaseShopController<ShopView>
    {
        private readonly ViewService _viewService;

        public ShopController(ShopView viewBase,
            IServerRequestSender serverRequestSender,
            PerksService perksService,
            ShopData data,
            WalletService walletService,
            ViewService viewService, TelegramLauncher telegramLauncher)
            : base(viewBase, 
                serverRequestSender, 
                perksService, 
                data, 
                walletService, telegramLauncher)
        {
            _viewService = viewService;
        }

        protected override void OnShow()
        {
            base.OnShow();
            View.OnAnyItemMenuOpened += OnChangeItemMenuState;
        }

        protected override void OnHide()
        {
            base.OnHide();
            View.OnAnyItemMenuOpened -= OnChangeItemMenuState;
        }

        private void OnChangeItemMenuState(bool value)
        {
            if (value)
                _viewService.HidePermanent<MenuListBarController>();
            else 
                _viewService.ShowPermanent<MenuListBar, MenuListBarController>();
        }
    }
}