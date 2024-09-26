using DG.Tweening;
using TMPro;
using UI.CustomButtons;
using UI.ViewService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{ 
    public class HudView : View
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _flashCount;
        [SerializeField] private string _coinAtlasCode = "<sprite name=\"Coin\">";
        [SerializeField] private TouchArea _touchArea;
        [SerializeField] private Button _gearButton;
        
        [Header("Animations")]
        [SerializeField] private Transform _rocketFlashIcon;
        [SerializeField] private float _rocketFlashDuration = 3f;
        
        public TouchArea TouchArea => _touchArea;
        public Button GearButton => _gearButton;
        
        public void Initialize(int coins) 
            => SetCoinsCount(coins);

        public void SetCoinsCount(int coins) 
            => _coinsText.text = $"{_coinAtlasCode}{coins.ToString("D6").Insert(3, " ")}";

        public override void OnShow()
        {
            AnimateRocketButton();
        }

        private void AnimateRocketButton()
        {
            _rocketFlashIcon.DORotate(new Vector3(0, 0, 360), _rocketFlashDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}