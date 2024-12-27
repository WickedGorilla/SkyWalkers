using Infrastructure.Data.Game.MiniGames.PasswordMiniGame;
using UI.Core;

namespace UI.Views
{
    public class PasswordViewController : ViewController<PasswordView>
    {
        private readonly PasswordMiniGameData _passwordMiniGameData;

        public PasswordViewController(PasswordView view, PasswordMiniGameData passwordMiniGameData) : base(view)
        {
            _passwordMiniGameData = passwordMiniGameData;
        }

        protected override void OnShow()
        {
            View.Initialize(_passwordMiniGameData.GetRandomPassword().NodesIndexes);
        }

        protected override void OnHide()
        {
            View.ResetPattern();
        }
    }
}