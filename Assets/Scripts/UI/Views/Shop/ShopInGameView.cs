using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ShopInGameView : ShopView
    {
        [SerializeField] private Button _backButton;
        
        public Button BackButton => _backButton;
    }
}