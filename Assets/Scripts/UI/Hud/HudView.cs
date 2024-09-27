using System.Collections;
using DG.Tweening;
using Game.Wallet.Flash;
using TMPro;
using UI.CustomButtons;
using UI.ViewService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class HudView : View
    {
        [Header("Coins")] 
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private string _coinAtlasCode = "<sprite name=\"Coin\">";

        [Header("Energy")] 
        [SerializeField] private TMP_Text _energyCount;
        [SerializeField] private Image _energyFiller;
        [SerializeField] private Button _energyButton;
        [SerializeField] private float _energyFillerDuration = 0.3f;

        [Header("Boost")] 
        [SerializeField] private TMP_Text _boostCountText;
        [SerializeField] private Button _boostButton;

        [SerializeField] private TouchArea _touchArea;
        [SerializeField] private Button _gearButton;

        [Header("Animations")] [SerializeField]
        private Transform _rocketFlashIcon;

        [SerializeField] private float _rocketFlashDuration = 3f;

        private Coroutine _energyFillerCoroutine;

        public TouchArea TouchArea => _touchArea;
        public Button GearButton => _gearButton;

        public void Initialize(int coins, RangeValue rangeValue, int flashEnergy)
        {
            SetCoinsCount(coins);
            FillEnergy(rangeValue.CurrentCount, rangeValue.MaxCount, flashEnergy);
        }

        public void FillEnergy(int current, int max, int flash)
        {
            _energyCount.text = $"{current}\n/{max}";
            var allMax = flash * max + max;
            var allCurrent = flash * max + current;

            if (_energyFillerCoroutine != null)
                StopCoroutine(_energyFillerCoroutine);

            float normalizedValue = allMax == 0 ? 0 : allCurrent / (float)allMax;
            _energyFillerCoroutine = StartCoroutine(AnimateEnergyLine(normalizedValue));
        }

        public void SetCoinsCount(int coins)
            => _coinsText.text = $"{_coinAtlasCode}{coins:N0}";

        public override void OnShow()
        {
            AnimateRocketButton();
        }

        private void AnimateRocketButton()
        {
            _rocketFlashIcon.DORotate(new Vector3(0, 0, 360f), _rocketFlashDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

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
    }
}