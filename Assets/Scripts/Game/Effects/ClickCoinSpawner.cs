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

        private PoolCollection<Image> _poolCoins;
        private PoolCollection<TMP_Text> _poolTextCount;

        private Transform _uiParent;

        public ClickCoinSpawner(CoinsSpawnerData data,
            ViewPrefabsData viewPrefabsData)
        {
            _data = data;
            _viewPrefabsData = viewPrefabsData;
        }

        public void Initialize()
        {
            _uiParent = Object.Instantiate(_viewPrefabsData.Root).transform;
            _poolCoins = new PoolCollection<Image>(_uiParent);
            _poolTextCount = new PoolCollection<TMP_Text>(_uiParent);
        }

        public void SpawnCoinEffect(int coinAmount, Vector2 centerPosition)
        {
            Vector2 spawnPosition = GetRandomPositionInsideArc(centerPosition);
            Vector2 direction = (spawnPosition - centerPosition).normalized;
            Vector2 target = spawnPosition + direction * _data.MoveDistance;
            float rotation = Random.Range(-30f, 30f);

            var spawnedCoin = _poolCoins.Get(_data.CoinPrefab, spawnPosition, rotation, _uiParent);
            AnimateCoin(spawnedCoin, target);
            SpawnText($"+{coinAmount}", spawnPosition, target, _data.TextAnimationDuration);
        }

        public void SpawnText(string text, Vector2 position, Vector2 endPosition, float duration)
        {
            var textPosition = position + _data.TextOffset;
            var spawnedText = _poolTextCount.Get(_data.TextPrefab, textPosition, 0f, _uiParent);
            spawnedText.text = text;
            AnimateText(spawnedText, endPosition, duration);
        }

        private void AnimateText(TMP_Text spawnedText, Vector2 target, float duration)
        {
            target += _data.TextOffset;

            Sequence textSequence = DOTween.Sequence();
            textSequence.Join(spawnedText.transform.DOMove(target, duration)
                .SetEase(Ease.OutCubic));

            textSequence.Join(spawnedText.DOFade(0, duration)
                .SetEase(Ease.InQuad));

            textSequence.OnComplete(() =>
            {
                spawnedText.color = GetResetAlfa(spawnedText.color);
                _poolTextCount.Return(spawnedText);
            });

            textSequence.Play();
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

        private void AnimateCoin(Image spawnedCoin, Vector2 target)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(spawnedCoin.transform.DOMove(target, _data.CoinAnimationDuration)
                .SetEase(Ease.OutCubic));

            sequence.Join(spawnedCoin.transform.DOScale(Vector3.zero, _data.CoinAnimationDuration)
                .SetEase(Ease.InOutCubic));

            sequence.Join(spawnedCoin.DOFade(0, _data.CoinAnimationDuration)
                .SetEase(Ease.InQuad));

            sequence.OnComplete(() =>
            {
                spawnedCoin.transform.localScale = Vector3.one;
                spawnedCoin.color = GetResetAlfa(spawnedCoin.color);
                _poolCoins.Return(spawnedCoin);
            });

            sequence.Play();    
        }

        private Color GetResetAlfa(Color color)
            => new(color.r, color.g, color.b) { a = 1f };
    }
}