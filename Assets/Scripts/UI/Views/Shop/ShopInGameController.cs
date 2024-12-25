using Game.Perks;
using Game.Validation;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network;
using Infrastructure.Telegram;
using Player;
using UI.Core;
using UI.Hud;

namespace UI.Views
{
    public class ShopInGameController : BaseShopController<ShopInGameView>
    {
        private readonly ViewService _viewService;
        private readonly CoinValidationService _coinValidationService;

        public ShopInGameController(ShopInGameView view,
            ViewService viewService, 
            IServerRequestSender serverRequestSender, 
            PerksService perksService, 
            ShopData shopData,
            WalletService walletService,
            CoinValidationService coinValidationService,
            TelegramLauncher telegramLauncher) 
            : base(view, 
                serverRequestSender,
                perksService,
                shopData, 
                walletService, 
                telegramLauncher)
        {
            _viewService = viewService;
            _coinValidationService = coinValidationService;
        }

        protected override void OnShow()
        {
            base.OnShow();
            View.BackButton.onClick.AddListener(OnClickBackButton);
            _coinValidationService.Stop();
        }

        protected override void OnHide()
        {
            base.OnHide();
            View.BackButton.onClick.RemoveListener(OnClickBackButton);
            _coinValidationService.Start();
        }

        private void OnClickBackButton() 
            => _viewService.Show<HudView, HudController>();
    }
}