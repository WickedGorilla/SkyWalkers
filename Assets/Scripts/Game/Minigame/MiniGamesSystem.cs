using System;
using System.Collections.Generic;
using Game.Player;
using Infrastructure.Actions;
using Infrastructure.Data.Game.MiniGames;
using Infrastructure.Disposables;
using Player;
using UI.Core;
using UI.Hud;
using UI.Views;
using UI.Views.MiniGames;
using UI.Views.MiniGames.RainView;
using UI.Views.MiniGames.SecurityGuardView;
using UI.Views.Timer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.MiniGames
{
    public class MiniGamesSystem
    {
        private readonly MiniGamesData _miniGamesData;
        private readonly WalletService _walletService;
        private readonly FarmCoinsSystem _farmCoinsSystem;
        private readonly ViewService _viewService;
        private readonly BoostSystem _boostSystem;
        private readonly Dictionary<MiniGameType, Func<IMiniGameViewController>> _miniGamesStartActions;
        
        private int _tapsCount;
        private int _countTapsToStartMiniGame;
        
        private IDisposable _miniGameDisposable;
        private IDisposable _tapCoinListener;
        private IMiniGameViewController _miniGameViewController;
        private IUpdateTimer _timer;

        public event Action<MiniGameType> OnEnterMiniGame;
        public event Action<bool> OnCompleteMiniGame;
        
        public MiniGamesSystem(MiniGamesData miniGamesData,
            WalletService walletService,
            FarmCoinsSystem farmCoinsSystem,
            ViewService viewService,
            BoostSystem boostSystem)
        {
            _miniGamesData = miniGamesData;
            _walletService = walletService;
            _farmCoinsSystem = farmCoinsSystem;
            _viewService = viewService;
            _boostSystem = boostSystem;
            _miniGamesStartActions = CreateMiniGamesStartActions();
            _countTapsToStartMiniGame = GetUpdateTapsToStartMiniGame();
        }
        
        public int EarnedCoinsBeforeMiniGame { get; private set; }
        
        public void OnStart()
        {
            _tapCoinListener = EnableMiniGameCounter();
            
            _boostSystem.OnUseBoost += OnBoostActivated;
            _boostSystem.OnEndBoost += OnBoostDeactivated;
        }

        public void OnStop()
        {
            _tapCoinListener.Dispose();
            
            _boostSystem.OnUseBoost -= OnBoostActivated;
            _boostSystem.OnEndBoost -= OnBoostDeactivated;
        }

        private void OnBoostActivated() 
            => _tapCoinListener.Dispose();

        private void OnBoostDeactivated() 
            => _tapCoinListener = EnableMiniGameCounter();
        
        private IDisposable EnableMiniGameCounter()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap += OnFarmCoins;
            
            return DisposableContainer.Create(Disable);

            void Disable() 
                => _farmCoinsSystem.OnFarmCoinsPerTap -= OnFarmCoins;
        }
        
        private void OnFarmCoins(int coins)
        {
            EarnedCoinsBeforeMiniGame += coins;
            _tapsCount++;

            if (_tapsCount >= _countTapsToStartMiniGame)
                StartMiniGame();
        }

        private async void StartMiniGame()
        {
            _viewService.HideCurrent();
            
            var enumType = typeof(MiniGameType);
            var maxIndex = Enum.GetValues(enumType).Length;
            var randomIndex = Random.Range(0, maxIndex);
            var miniGameType = (MiniGameType)randomIndex;

            miniGameType = MiniGameType.Rain;
            
            if (!_miniGamesStartActions.TryGetValue(miniGameType, out Func<IMiniGameViewController> miniGameStartAction))
                throw new KeyNotFoundException("Unknown mini game type");

            OnEnterMiniGame?.Invoke(miniGameType);
            
            await Awaitable.WaitForSecondsAsync(_miniGamesData.DelayToStartMiniGame);
            
            _miniGameViewController = miniGameStartAction();
            _timer = _miniGameViewController.CreateTimer(OnTimeLeft);
            _timer.Start();
            
            _miniGameDisposable = SubscribeController(_miniGameViewController);
        }

        private void OnTimeLeft()
        {
            if (_miniGameViewController.CheckIsComplete())
                return;

            _miniGameViewController.DoFailMiniGame();
        }

        private IDisposable SubscribeController(IMiniGameViewController controller)
        {
            controller.OnCompleteMiniGame += OnMiniGameComplete;
            controller.OnFailMiniGame += OnMiniGameFail;
            
            return DisposableContainer.Create(() =>
            {
                controller.OnCompleteMiniGame -= OnMiniGameComplete;
                controller.OnFailMiniGame -= OnMiniGameFail;
            });
        }
        
        private void OnMiniGameFail(IEventAwaiter animationAwaiter)
        {
            var subtractValue = EarnedCoinsBeforeMiniGame / 2;
            _walletService.Coins.Subtract(subtractValue);
            _timer.Stop();
            
            OnCompleteMiniGame?.Invoke(false);
            
            animationAwaiter.AddAwaiter(() =>
            {
                _viewService.Show<HudView, HudController>();
                ResetMiniGame();
            });
        }

        private void OnMiniGameComplete(IEventAwaiter animationAwaiter)
        {
            _timer.Stop();
            OnCompleteMiniGame?.Invoke(true);

            animationAwaiter.AddAwaiter(() =>
            {
                _viewService.Show<HudView, HudController>();
                ResetMiniGame();
            });
        }

        private void ResetMiniGame()
        {
            _countTapsToStartMiniGame = GetUpdateTapsToStartMiniGame();
            EarnedCoinsBeforeMiniGame = 0;
            _tapsCount = 0;

            _miniGameDisposable.Dispose();
            _miniGameDisposable = null;
        }

        private Dictionary<MiniGameType, Func<IMiniGameViewController>> CreateMiniGamesStartActions()
        {
            return new Dictionary<MiniGameType, Func<IMiniGameViewController>>
            {
                [MiniGameType.Password] = () => _viewService.Show<PasswordView, PasswordViewController>(),
                [MiniGameType.SecurityGuard] = () => _viewService.Show<SecurityGuardView, SecurityGuardViewController>(),
                [MiniGameType.Rain] = () => _viewService.Show<RainView, RainViewController>(),
            };
        }

        private int GetUpdateTapsToStartMiniGame()
        {
            Vector2Int value = _miniGamesData.RangeTapsToStartMiniGame;
            return Random.Range(value.x, value.y);
        }
    }
}