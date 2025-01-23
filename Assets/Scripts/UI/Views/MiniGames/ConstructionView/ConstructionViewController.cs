using System;
using System.Collections.Generic;
using System.Threading;
using Game.Environment;
using Infrastructure.Actions;
using Infrastructure.Data.Game.MiniGames.ConstructionMiniGame;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Views.MiniGames.ConstructionView
{
    public class ConstructionViewController : ViewController<ConstructionView>, IMiniGameViewController
    {
        private readonly LinkedList<KeyValuePair<ButtonDirectionType, float>> _directionsForButtons = new();
        private readonly ConstructionMiniGameData _miniGameData;
        private readonly IEnvironmentHolder _environmentHolder;

        private ButtonDirectionType _currentDirection;
        private float _currentPower;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isComplete;
        private Vector3 _defaultConstructionPosition;

        public ConstructionViewController(ConstructionView view,
            ConstructionMiniGameData miniGameData,
            IEnvironmentHolder environmentHolder) : base(view)
        {
            _miniGameData = miniGameData;
            _environmentHolder = environmentHolder;
        }

        public event Action<IEventAwaiter> OnCompleteMiniGame;
        public event Action<IEventAwaiter> OnFailMiniGame;

        public EnvironmentAnimation Platform => _environmentHolder.Environment.PlatformConstruction;

        public bool CheckIsComplete()
            => _isComplete;

        public void DoFailMiniGame()
        {
            var eventAwaiter = new EventAwaiter();
            View.VisualizeFail(eventAwaiter.Complete);
            OnFailMiniGame?.Invoke(eventAwaiter);
            EndMiniGame();
        }

        public IUpdateTimer CreateTimer(Action onTimeLeft)
            => View.Timer.CreateTimer(_miniGameData.TimeForMiniGame, onTimeLeft);

        protected override void OnShow()
        {
            _defaultConstructionPosition = Platform.transform.position;

            Initialize();
            Platform.DoShow();

            _cancellationTokenSource = new CancellationTokenSource();
            OnUpdate(_cancellationTokenSource);
        }

        protected override void OnHide()
        {
            _directionsForButtons.Clear();
            Platform.transform.position = _defaultConstructionPosition;
        }

        private async void OnUpdate(CancellationTokenSource tokenSource)
        {
            while (!tokenSource.IsCancellationRequested)
            {
                UpdateInput();
                await Awaitable.NextFrameAsync();
            }
        }

        private void Initialize()
        {
            float reservePower = _miniGameData.ReservePowerControl;

            while (reservePower > 0f)
            {
                if (GetRandomBool())
                {
                    var stackValue = GetPower(_miniGameData.PowerRangeForRight);
                    _directionsForButtons.AddLast(
                        new KeyValuePair<ButtonDirectionType, float>(ButtonDirectionType.Right, stackValue));
                    reservePower -= stackValue;
                }
                else
                {
                    var stackValue = GetPower(_miniGameData.PowerRangeForLeft);
                    _directionsForButtons.AddLast(
                        new KeyValuePair<ButtonDirectionType, float>(ButtonDirectionType.Left, stackValue));
                    reservePower -= stackValue;
                }
            }

            UpdateDirectionButton();

            float GetPower(Vector2 range)
                => Random.Range(range.x, range.y);

            bool GetRandomBool()
                => Random.Range(0, 2) == 0;
        }

        private void UpdateInput()
        {
            if (_currentDirection == ButtonDirectionType.Right)
            {
                if (!View.RightButton.IsPressed)
                    return;

                MovePlatform();
            }
            else
            {
                if (!View.LeftButton.IsPressed)
                    return;

                MovePlatform(0.5f);
            }
        }

        private void MovePlatform(float powerMultiplier = 1f)
        {
            var applyPower = _miniGameData.ForceDecayRate * Time.deltaTime;

            if (applyPower > _currentPower)
                applyPower = _currentPower;

            _currentPower -= applyPower;

            var platformPos = Platform.transform.position;
            Platform.transform.position = new Vector3(platformPos.x + powerMultiplier * applyPower,
                platformPos.y,
                platformPos.z);

            if (_currentPower < 0.0001f)
                UpdateDirectionButton();
        }

        private void UpdateDirectionButton()
        {
            if (_directionsForButtons.Count == 0)
            {
                DoSuccess();
                return;
            }

            var value = _directionsForButtons.First.Value;
            _currentDirection = value.Key;
            _currentPower = value.Value;
            View.HighlightButton(_currentDirection);

            _directionsForButtons.RemoveFirst();
        }

        private void DoSuccess()
        {
            _isComplete = true;
            EndMiniGame();

            var eventAwaiter = new EventAwaiter();
            View.VisualizeSuccess(eventAwaiter.Complete);
            OnCompleteMiniGame?.Invoke(eventAwaiter);
        }

        private void EndMiniGame()
        {
            _cancellationTokenSource.Cancel();
            Platform.DoHide();
        }

        public enum ButtonDirectionType
        {
            Right,
            Left
        }
    }
}