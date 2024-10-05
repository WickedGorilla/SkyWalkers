using UI.Views;
using UI.Core;

namespace Game.Infrastructure
{
    public class MainMenuState : IState
    {
        private readonly ViewService viewService;

        private MenuListBarController _listBarController;

        public MainMenuState(ViewService viewService)
        {
            this.viewService = viewService;
        }

        public void Enter()
        {
            viewService.Show<MainMenuView, MainMenuController>();
            _listBarController = viewService.ShowPermanent<MenuListBar, MenuListBarController>();
        }

        public void Exit()
        {
            viewService.HidePermanent(_listBarController);
        }
        

    }
}