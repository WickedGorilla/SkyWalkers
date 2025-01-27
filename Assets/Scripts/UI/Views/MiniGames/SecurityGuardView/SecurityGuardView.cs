using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.PoolSystem;
using Infrastructure;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.MiniGames.SecurityGuardView
{
    public class SecurityGuardView : View
    {
        private readonly LinkedList<RectTransform> _spawnedFarmCoins = new();

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private ViewTimer _timer;

        [Header("Pool objects")] 
        [SerializeField] private float _offsidePoint;

        [SerializeField] private float _offsetObjects;
        [SerializeField] private float _movementSpeed;

        [Header("Hit animation")] 
        [SerializeField] private float _moveUpDistance = 200f;

        [SerializeField] private float _durationUpMovement = 1f;

        [Header("Back animation")] 
        [SerializeField] private float _minAlpha = 0.2f;

        [SerializeField] private float _maxAlpha = 0.4f;
        [SerializeField] private float _duration = 1f;

        [SerializeField] private RectTransform _farmCoinPrefab;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private Image _circleCastImage;
        [SerializeField] private RectTransform _castCircle;

        [Header("Colors")] 
        [SerializeField] private Color _mistakeColor = Color.red;
        [SerializeField] private Color _successColor = Color.green;

        private PoolObjects<RectTransform> _farmCoinsPool;
        
        private float _spawnXPosition;
        private float _maxXPosition;
        private int _currentFrame;
        
        private bool _isMovementRoutine;
        private Coroutine _movementRoutine;
        
        public ViewTimer Timer => _timer;

        public event Action OnEarnSuccess;
        public event Action OnEarnMistake;

        private void Awake()
        {
            _farmCoinsPool = new PoolObjects<RectTransform>(_farmCoinPrefab, _parent);
            _spawnXPosition = _parent.localPosition.x - _offsidePoint;
            _maxXPosition = _parent.localPosition.x + _offsidePoint;
        }

        public override void OnShow()
        {
            _currentFrame = default;
            AnimateAlpha(-1);
            SpawnFirst();

            _isMovementRoutine = true;
            _movementRoutine = StartCoroutine(MovementRoutine());
        }

        public override void OnHide()
        {
            _backgroundImage.DOKill();
            ClearAllObjects();
            StopMovement();
        }

        private IEnumerator MovementRoutine()
        {
            while (_isMovementRoutine)
            {
                MoveObjects();
                CastCircle();
                yield return null;
            }
        }

        private void StopMovement()
        {
            if (!_isMovementRoutine)
                return;
            
            _isMovementRoutine = false;
            StopCoroutine(_movementRoutine);
        }
        
        private void CastCircle()
        {
            if (!ScreenInput.GetTouchDown())
                return;

            var currentNode = _spawnedFarmCoins.First;

            Vector3[] castCorners = new Vector3[4];
            Vector3[] borders = new Vector3[4];

            _castCircle.GetWorldCorners(castCorners);

            while (currentNode != null)
            {
                currentNode.Value.GetWorldCorners(borders);

                if (CheckCornersIsBefore(castCorners, borders))
                {
                    DoMistake();
                    break;
                }

                if (CheckCornersIsHit(castCorners, borders))
                {
                    DoHit(currentNode);
                    return;
                }

                currentNode = currentNode.Next;
            }

            DoMistake();
        }

        private void DoMistake()
        {
            DoMistakeEffect();
            OnEarnMistake?.Invoke();
        }

        private bool CheckCornersIsBefore(Vector3[] castCorners, Vector3[] objCorners)
            => objCorners[1].x < castCorners[0].x;

        private bool CheckCornersIsHit(Vector3[] castCorners, Vector3[] objCorners)
        {
            if (objCorners[0].x < castCorners[0].x || objCorners[2].x > castCorners[2].x)
                return false;

            return true;
        }

        private void DoHit(LinkedListNode<RectTransform> node)
        {
            _spawnedFarmCoins.Remove(node);
            DoFarmEffect(node.Value);
            OnEarnSuccess?.Invoke();
        }
        
        private void DoMistakeEffect()
            => DoCircleEffect(_mistakeColor);

        private void DoCircleEffect(Color color)
        {
            var initialColor = _circleCastImage.color;
            
             _circleCastImage.DOColor(color, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnKill(() => _circleCastImage.color = initialColor);
        }
        
        private void DoFarmEffect(RectTransform rectTransform)
        {
            DoCircleEffect(_successColor);

            var image = rectTransform.GetComponent<Image>();
            Sequence sequence = DOTween.Sequence();

            var moveTweener = rectTransform
                .DOAnchorPos(rectTransform.anchoredPosition + new Vector2(0, _moveUpDistance),
                    _durationUpMovement)
                .SetEase(Ease.OutQuad);

            var alphaTweener = image.DOFade(0, _durationUpMovement)
                .SetEase(Ease.OutQuad);

            sequence.Append(moveTweener);
            sequence.Join(alphaTweener);
            sequence.OnComplete(() =>
            {
                var color = image.color;
                color.a = 1f;
                image.color = color;

                _farmCoinsPool.Return(rectTransform);
            });
        }

        private void SpawnFirst()
        {
            RectTransform first = _farmCoinsPool.Get(_parent);
            first.localPosition = new Vector3(_spawnXPosition, 0f, 0f);
            _spawnedFarmCoins.AddFirst(first);
        }

        private void AnimateAlpha(int loops)
        {
            _backgroundImage.enabled = true;
            Color initialColor = _backgroundImage.color;
            initialColor.a = _maxAlpha;
            _backgroundImage.color = initialColor;

            _backgroundImage.DOFade(_minAlpha, _duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(loops, LoopType.Yoyo)
                .OnComplete(() => _backgroundImage.enabled = false);
        }

        private void MoveObjects()
        {
            foreach (RectTransform coin in _spawnedFarmCoins)
            {
                Vector3 currentPos = coin.localPosition;
                Vector3 newPosition = new Vector3(currentPos.x + Time.deltaTime * _movementSpeed, 0f, 0f);
                coin.localPosition = newPosition;
            }

            _currentFrame++;

            if (_currentFrame % 3 != 0)
                return;

            CheckAndRemoveFirst();
            CheckAndSpawnLast();
        }

        private void CheckAndSpawnLast()
        {
            if (_spawnedFarmCoins.Last is not null)
            {
                RectTransform last = _spawnedFarmCoins.Last.Value;
                Vector3 lastPosition = last.localPosition;

                if (lastPosition.x > _spawnXPosition)
                {
                    var newPosition = lastPosition.x - _offsetObjects;
                    SpawnLast(newPosition);
                }
            }
        }

        private void SpawnLast(float xPosition)
        {
            var newObject = _farmCoinsPool.Get(_parent);
            newObject.transform.localPosition = new Vector3(xPosition, 0f, 0f);
            _spawnedFarmCoins.AddLast(newObject);
        }

        private void CheckAndRemoveFirst()
        {
            var obj = _spawnedFarmCoins.First.Value;

            if (obj.localPosition.x > _maxXPosition)
            {
                _farmCoinsPool.Return(obj);
                _spawnedFarmCoins.RemoveFirst();
            }
        }

        private void ClearAllObjects()
        {
            foreach (var obj in _spawnedFarmCoins)
                _farmCoinsPool.Return(obj);

            _spawnedFarmCoins.Clear();
        }

        public void VisualizeFail(Action onComplete)
        {
            StopMovement();
            
            _backgroundImage.DOColor(_mistakeColor, _duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
        
        public void VisualizeSuccess(Action onComplete)
        {
            StopMovement();
            
            _backgroundImage.DOColor(_successColor, _duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
    }
}