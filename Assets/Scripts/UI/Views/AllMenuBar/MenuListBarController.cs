using UI.Core;

namespace UI.Views
{
    public class MenuListBarController : ViewController<MenuListBar>
    {
        private readonly ViewService _viewService;

        public MenuListBarController(ViewService viewService, MenuListBar view) : base(view)
        {
            _viewService = viewService;
        }

        protected override void OnShow()
        {
            View.HomeButton.onClick.AddListener(Show<MainMenuView, MainMenuController>);
            View.InviteButton.onClick.AddListener(Show<InviteView, InviteController>);
            View.QuestButton.onClick.AddListener(Show<QuestView, QuestController>);
            View.ShopButton.onClick.AddListener(Show<ShopView, ShopController>);
            View.WalletButton.onClick.AddListener(Show<WalletView, WalletController>);
        }

        protected override void OnHide()
        {
            View.HomeButton.onClick.RemoveListener(Show<MainMenuView, MainMenuController>);
            View.InviteButton.onClick.RemoveListener(Show<InviteView, InviteController>);
            View.QuestButton.onClick.RemoveListener(Show<QuestView, QuestController>);
            View.ShopButton.onClick.RemoveListener(Show<ShopView, ShopController>);
            View.WalletButton.onClick.RemoveListener(Show<WalletView, WalletController>);
        }

        private void Show<TView, TController>()
            where TView : View where TController : ViewController<TView>
            => _viewService.Show<TView, TController>();
    }
}