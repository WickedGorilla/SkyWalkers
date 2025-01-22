using UI.Core;
using UI.CustomButtons;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.MiniGames.ConstructionView
{
    public class ConstructionView : View
    {
        [SerializeField] private ViewTimer _timer;
        [SerializeField] private UIButton _leftButton;
        [SerializeField] private UIButton _rightButton;
        [SerializeField] private Image _leftImage;
        [SerializeField] private Image _rightImage;
        [SerializeField] private Color _disableColor = Color.gray;

        private Image _selectedButton;
        
        public ViewTimer Timer => _timer;
        public UIButton LeftButton => _leftButton;
        public UIButton RightButton => _rightButton;
        
        public void HighlightButton(ConstructionViewController.ButtonDirectionType buttonDirectionType)
        {
            HighlightButton(buttonDirectionType == ConstructionViewController.ButtonDirectionType.Right
                ? _rightImage
                : _leftImage);
        }

        private void HighlightButton(Image imageButton)
        {
            if (_selectedButton != null)
                _selectedButton.color = _disableColor;
            
            _selectedButton = imageButton;
            _selectedButton.color = Color.white;
        }
    }
}