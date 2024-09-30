using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MenuListBar : View
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _inviteButton;
        [SerializeField] private Button _questButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _walletButton;
        
        public Button HomeButton => _homeButton;
        public Button InviteButton => _inviteButton;
        public Button QuestButton => _questButton;
        public Button ShopButton => _shopButton;
        public Button WalletButton => _walletButton;
    }
}