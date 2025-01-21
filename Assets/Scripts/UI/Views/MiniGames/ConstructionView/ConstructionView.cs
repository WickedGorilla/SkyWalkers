using UI.Core;
using UI.CustomButtons;
using UI.Views.Timer;
using UnityEngine;

namespace UI.Views.MiniGames.ConstructionView
{
    public class ConstructionView : View
    {
        [SerializeField] private ViewTimer _timer;
        [SerializeField] private UIButton _leftButton;
        [SerializeField] private UIButton _rightButton;
        
        public ViewTimer Timer => _timer;

        public UIButton LeftButton => _leftButton;
        public UIButton RightButton => _rightButton;
        
        public override void OnShow()
        {
            
        }

        public override void OnHide()
        {
            
        }
    }
}