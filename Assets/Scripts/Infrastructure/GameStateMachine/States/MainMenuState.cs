using UI.Views;
using UI.Core;

namespace Game.Infrastructure
{
    public class MainMenuState : IState
    {
        private readonly ViewService _viewService;

        public MainMenuState(ViewService viewService)
        {
            _viewService = viewService;
        }

        public void Enter()
        {
            _viewService.Show<MainMenuView, MainMenuController>();
            _viewService.ShowPermanent<MenuListBar, MenuListBarController>();
        }

        public void Exit()
        {
            _viewService.HidePermanent<MenuListBarController>();
        }
    }
}