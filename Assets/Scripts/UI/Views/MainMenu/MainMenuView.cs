using DG.Tweening;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MainMenuView : View
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _languageCode;
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private Image _userPicture;

        [Header("Buttons")] 
        [SerializeField] private Button _playButton;

        [Header("Update coins count Animation params")] 
        [SerializeField] private float pulseFontSize = 60f;

        [SerializeField] private float pulseDuration = 0.2f;
        [SerializeField] private float increaseDuration = 1.0f;

        private int _currentCoins;
        private float _originalFontSize;

        public Button PlayButton => _playButton;

        public void Initialize(int coins, string nickname, string languageCode)
        {
            _currentCoins = coins;
            _languageCode.text = languageCode;
            SetCoinsCount(_currentCoins);
            _nicknameText.text = nickname;
        }

        public override void OnShow() 
            => _originalFontSize = _coinsText.fontSizeMax;

        public override void OnHide()
            => ResetAnimations();

        public void UpdateCoins(int newValue)
        {
            DOTween.To(() => _currentCoins, x =>
                {
                    _currentCoins = x;
                    SetCoinsCount(_currentCoins);
                }, newValue, increaseDuration)
                .OnStart(PulseFontSize);

            void PulseFontSize()
            {
                DOTween.To(() => _coinsText.fontSizeMax, x => _coinsText.fontSizeMax = x, pulseFontSize, pulseDuration)
                    .SetLoops(1, LoopType.Yoyo)
                    .OnComplete(ResetAnimations);
            }
        }

        public void SetPicture(Sprite sprite)
            => _userPicture.sprite = sprite;

        private void SetCoinsCount(int coins)
            => _coinsText.text = NumbersFormatter.GetCoinsCountVariant(coins);

        private void ResetAnimations()
            => _coinsText.fontSizeMax = _originalFontSize;
    }
}