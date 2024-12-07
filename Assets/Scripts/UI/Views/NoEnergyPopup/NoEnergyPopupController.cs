using SkyExtensions;
using UI.Core;

namespace UI.Views
{
    public class NoEnergyPopupController : ViewController<NoEnergyPopup>
    {
        private readonly ViewService _viewService;

        public NoEnergyPopupController(NoEnergyPopup view, ViewService viewService) : base(view)
        {
            _viewService = viewService;
        }

        protected override void OnShow()
        {
            View.BuyButton.AddClickAction(OnClickShop);
            View.WaitCloseButton.AddClickAction(Hide);
        }

        protected override void OnHide()
        {
            View.BuyButton.RemoveClickAction(OnClickShop);
            View.WaitCloseButton.RemoveClickAction(Hide);
        }

        private void OnClickShop()
        {
            Hide();
            _viewService.Show<ShopInGameView, ShopInGameController>();
        }
    }
}