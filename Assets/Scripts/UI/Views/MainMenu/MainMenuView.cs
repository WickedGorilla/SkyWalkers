using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MainMenuView : View
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private string _coinAtlasCode = "<sprite name=\"Coin\">";
        [SerializeField] private Image _userPicture;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        
        public Button PlayButton => _playButton;
        
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

        public void SetPicture(Sprite sprite) 
            => _userPicture.sprite = sprite;

        public void SetCoinsCount(int coins) 
            => _coinsText.text = $"{_coinAtlasCode}{coins:N0}";
    }
}