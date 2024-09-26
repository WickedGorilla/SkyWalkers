using UI.MainMenu;
using UI.ViewService;

namespace Game.Infrastructure
{
    public class MainMenuState : IState
    {
        private readonly ViewService viewService;

        public MainMenuState(ViewService viewService)
        {
            this.viewService = viewService;
        }
        
        public void Enter()
        {
            viewService.Show<MainMenuView, MainMenuController>();
        }

        public void Exit()
        {
   
        }
    }
}