using System.Collections;
using System.Globalization;
using DG.Tweening;
using Game.Wallet.Flash;
using TMPro;
using UI.CustomButtons;
using UI.Core;
using UI.Views;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class HudView : View
    {
        [Header("Coins")]
        [SerializeField] private TMP_Text _coinsText;

        [Header("Energy")]
        [SerializeField] private TMP_Text _energyCount;
        [SerializeField] private TMP_Text _energyFlashCount;
        [SerializeField] private Image _energyFiller;
        [SerializeField] private Button _energyButton;
        [SerializeField] private float _energyFillerDuration = 0.3f;

        [Header("Boost")]
        [SerializeField] private TMP_Text _boostCountText;
        [SerializeField] private Button _boostButton;
        [SerializeField] private RectTransform _boostLightingButton;
        [SerializeField] private RectTransform _boostTimerLabel;

        [SerializeField] private TouchArea _touchArea;
        [SerializeField] private Button _gearButton;
        [SerializeField] private Button _backButton;

        [Header("Animations")] 
        [SerializeField] private RectTransform _rocketFlashIcon;
        [SerializeField] private float _rocketFlashDuration = 3f;

        [Header("Boost Mode")]
        [SerializeField] private GameObject _defaultGroup;
        [SerializeField] private GameObject _boostGroup;
        [SerializeField] private ViewTimer _viewTimer;
        
        private Coroutine _energyFillerCoroutine;

        public TouchArea TouchArea => _touchArea; 
        public Button GearButton => _gearButton;
        public Button BoostButton => _boostButton;
        public Button EnergyButton => _energyButton;
        public Button BackButton => _backButton;
        public ViewTimer Timer => _viewTimer;
        public TMP_Text CoinsText => _coinsText;

        public override void OnShow()
        {
            AnimateRocketButton();
        }

        public void Initialize(int coins, RangeValue rangeValue, int flashEnergy, int boosts)
        {
            SetCoinsCount(coins);
            SetBoostCount(boosts);
            FillEnergy(rangeValue.CurrentCount, rangeValue.MaxCount, flashEnergy);
        }

        public void FillEnergy(int current, int max, int flashEnergy)
        {
            _energyCount.text = $"{current}\n/{max}";
            _energyFlashCount.text = $"{flashEnergy}";
            var allMax = flashEnergy * max + max;
            var allCurrent = flashEnergy * max + current;

            if (_energyFillerCoroutine != null)
                StopCoroutine(_energyFillerCoroutine);

            float normalizedValue = allMax == 0 ? 0 : allCurrent / (float)allMax;
            _energyFillerCoroutine = StartCoroutine(AnimateEnergyLine(normalizedValue));
        }

        public void SetCoinsCount(int coins)
            => _coinsText.text = NumbersFormatter.GetCoinsCountVariant(coins);

        public void EnableBoost(bool value)
        {
            _defaultGroup.SetActive(!value);
            _boostGroup.SetActive(value);
            BoostButton.interactable = !value;

            if (value)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(_boostTimerLabel.DOShakeAnchorPos(0.5f, 10f, 30, 90));
                sequence.SetLoops(-1, LoopType.Restart);
                sequence.SetDelay(3f);
                sequence.Play();
                
                DoCycleRotateZ(_boostLightingButton);
            }
        }

        public void SetBoostCount(int boosts) 
            => _boostCountText.text = $"{boosts}";

        private void AnimateRocketButton() 
            => DoCycleRotateZ(_rocketFlashIcon);

        private IEnumerator AnimateEnergyLine(float value)
        {
            float startValue = _energyFiller.fillAmount;
            float timeElapsed = 0f;

            while (timeElapsed < _energyFillerDuration)
            {
                timeElapsed += Time.deltaTime;
                _energyFiller.fillAmount = Mathf.Lerp(startValue, value, timeElapsed / _energyFillerDuration);
                yield return null;
            }

            _energyFiller.fillAmount = value;
        }

        private void DoCycleRotateZ(RectTransform transform)
        {
            transform.DORotate(new Vector3(0, 0, 360f), _rocketFlashDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}