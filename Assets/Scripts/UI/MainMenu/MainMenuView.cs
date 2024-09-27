using TMPro;
using UI.ViewService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuView : View
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private string _coinAtlasCode = "<sprite name=\"Coin\">";
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _inviteButton;
        [SerializeField] private Button _questButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _walletButton;
        
        public Button PlayButton => _playButton;
        public Button HomeButton => _homeButton;
        public Button InviteButton => _inviteButton;
        public Button QuestButton => _questButton;
        public Button ShopButton => _shopButton;
        public Button WalletButton => _walletButton;

        public void Initialize(int coins, string nickname)
        {
            SetCoinsCount(coins);
            _nicknameText.text = nickname;
        }

        public override void OnShow()
        {
            
        }

        public override void OnHide()
        {
        }

        public void SetCoinsCount(int coins) 
            => _coinsText.text = $"{_coinAtlasCode}{coins:N0}";
    }
}