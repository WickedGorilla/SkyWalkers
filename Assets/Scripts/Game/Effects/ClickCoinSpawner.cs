using DG.Tweening;
using Game.PoolSystem;
using Infrastructure.Data;
using Infrastructure.Data.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class ClickCoinSpawner
    {
        private readonly CoinsSpawnerData _data;
        private readonly ViewPrefabsData _viewPrefabsData;
        private readonly PoolCollection<Image> _poolCoins;
        private readonly PoolCollection<TMP_Text> _poolTextCount;

        private Transform _uiParent;

        public ClickCoinSpawner(CoinsSpawnerData data,
            ViewPrefabsData viewPrefabsData)
        {
            _data = data;
            _viewPrefabsData = viewPrefabsData;
            _poolCoins = new PoolCollection<Image>();
            _poolTextCount = new PoolCollection<TMP_Text>();
        }

        public void Initialize()
            => _uiParent = Object.Instantiate(_viewPrefabsData.Root).transform;

        public void SpawnCoinEffect(int coinAmount, Vector2 centerPosition)
        {
            for (int i = 0; i < coinAmount; i++)
            {
                Vector2 spawnPosition = GetRandomPositionInsideArc(centerPosition);
                Vector2 direction = (spawnPosition - centerPosition).normalized;
                Vector2 target = spawnPosition + direction * _data.MoveDistance;
                float rotation = Random.Range(-30f, 30f);

                var spawnedCoin = _poolCoins.Get(_data.CoinPrefab, spawnPosition, rotation, _uiParent);
                AnimateCoin(spawnedCoin, target);

                var textPosition = spawnPosition + _data.TextOffset;
                var spawnedText = _poolTextCount.Get(_data.TextPrefab, textPosition, 0f, _uiParent);
                spawnedText.text = $"+{coinAmount}";
                AnimateText(spawnedText, target);
            }
        }

        private Vector3 GetRandomPositionInsideArc(Vector3 center)
        {
            float angle = Random.Range(_data.StartAngle, _data.EndAngle);
            float radians = angle * Mathf.Deg2Rad;
            float radius = Random.Range(_data.MinRadius, _data.MaxRadius);
            float x = Mathf.Cos(radians) * radius;
            float y = Mathf.Sin(radians) * radius;
            
            return new Vector3(center.x + x, center.y + y, center.z);
        }
        
        private void AnimateText(TMP_Text spawnedText, Vector2 target)
        {
            target += _data.TextOffset;
            
            Sequence textSequence = DOTween.Sequence();
            textSequence.Join(spawnedText.transform.DOMove(target, _data.TextAnimationDuration)
                    .SetEase(Ease.OutCubic))
                .Join(spawnedText.DOFade(0, _data.TextAnimationDuration)
                    .SetEase(Ease.InQuad))
                .OnComplete(() =>
                {
                    spawnedText.color = GetResetColor(spawnedText.color);
                    _poolTextCount.Return(spawnedText);
                });

            textSequence.Play();
        }

        private void AnimateCoin(Image spawnedCoin, Vector2 target)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(spawnedCoin.transform.DOMove(target, _data.CoinAnimationDuration)
                    .SetEase(Ease.OutCubic))
                .Join(spawnedCoin.transform.DOScale(Vector3.zero, _data.CoinAnimationDuration)
                    .SetEase(Ease.InOutCubic))
                .Join(spawnedCoin.DOFade(0, _data.CoinAnimationDuration)
                    .SetEase(Ease.InQuad))
                .OnComplete(() =>
                {
                    spawnedCoin.color = GetResetColor(spawnedCoin.color);
                    _poolCoins.Return(spawnedCoin);
                });

            sequence.Play();
        }

        private Color GetResetColor(Color color)
        {
            var resetColor = new Color(color.r, color.g, color.b)
            {
                a = 1f
            };
            
            return resetColor;
        }
    }
}