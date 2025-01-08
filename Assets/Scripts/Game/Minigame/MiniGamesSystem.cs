using System;
using System.Collections.Generic;
using Game.Player;
using Infrastructure.Data.Game.MiniGames;
using Infrastructure.Disposables;
using Player;
using UI.Core;
using UI.Views;
using UI.Views.MiniGames;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Minigames
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

        private void StartMiniGame()
        {
            var enumType = typeof(MiniGameType);
            var maxIndex = Enum.GetValues(enumType).Length;
            var randomIndex = Random.Range(0, maxIndex);
            var miniGameType = (MiniGameType)randomIndex;
            
            if (!_miniGamesStartActions.TryGetValue(miniGameType, out Func<IMiniGameViewController> miniGameStartAction))
                throw new KeyNotFoundException("Unknown mini game type");

            _miniGameDisposable = SubscribeController(miniGameStartAction());
            OnEnterMiniGame?.Invoke(miniGameType);
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
        
        private void OnMiniGameFail()
        {
            var subtractValue = EarnedCoinsBeforeMiniGame / 2;
            _walletService.Coins.Subtract(subtractValue);
            
            OnCompleteMiniGame?.Invoke(false);
            
            ResetMiniGame();
        }

        private void OnMiniGameComplete()
        {
            OnCompleteMiniGame?.Invoke(true);
            
            ResetMiniGame();
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
            };
        }

        private int GetUpdateTapsToStartMiniGame()
        {
            Vector2Int value = _miniGamesData.RangeTapsToStartMiniGame;
            return Random.Range(value.x, value.y);
        }
    }
}