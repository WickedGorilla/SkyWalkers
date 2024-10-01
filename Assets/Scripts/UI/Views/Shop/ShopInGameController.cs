using UI.Core;
using UI.Hud;

namespace UI.Views
{
    public class ShopInGameController : BaseShopController<ShopInGameView>
    {
        private readonly ViewService _viewService;

        public ShopInGameController(ShopInGameView view, ViewService viewService) : base(view)
        {
            _viewService = viewService;
        }

        protected override void OnShow()
        {
            base.OnShow();
            View.BackButton.onClick.AddListener(OnClickBackButton);
        }

        protected override void OnHide()
        {
            base.OnHide();
            View.BackButton.onClick.RemoveListener(OnClickBackButton);
        }

        private void OnClickBackButton() 
            => _viewService.Show<HudView, HudController>();
    }
}