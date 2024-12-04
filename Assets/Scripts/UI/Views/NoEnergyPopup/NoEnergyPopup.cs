using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class NoEnergyPopup : View
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _waitCloseButton;

        public Button BuyButton => _buyButton;
        public Button WaitCloseButton => _waitCloseButton;
    }
}