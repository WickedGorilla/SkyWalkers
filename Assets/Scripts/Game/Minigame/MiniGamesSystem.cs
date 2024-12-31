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
        private readonly Dictionary<MiniGameType, Func<IMiniGameViewController>> _miniGamesStartActions;

        private int _earnedCoinsBeforeMiniGame;
        private int _tapsCount;
        private int _countTapsToStartMiniGame;
        private IDisposable _miniGameDisposable;
        
        public MiniGamesSystem(MiniGamesData miniGamesData,
            WalletService walletService,
            FarmCoinsSystem farmCoinsSystem,
            ViewService viewService)
        {
            _miniGamesData = miniGamesData;
            _walletService = walletService;
            _farmCoinsSystem = farmCoinsSystem;
            _viewService = viewService;
            _miniGamesStartActions = CreateMiniGamesStartActions();
            _countTapsToStartMiniGame = GetUpdateTapsToStartMiniGame();
        }

        public void OnStart()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap += OnFarmCoins;
        }

        public void OnStop()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap -= OnFarmCoins;
        }

        private void OnFarmCoins(int coins)
        {
            _earnedCoinsBeforeMiniGame += coins;
            _tapsCount++;

            if (_tapsCount >= _countTapsToStartMiniGame)
                StartMiniGame();
        }

        private void StartMiniGame()
        {
            var enumType = typeof(MiniGameType);
            var maxIndex = Enum.GetValues(enumType).Length;
            var randomIndex = Random.Range(0, maxIndex);

            if (!_miniGamesStartActions.TryGetValue((MiniGameType)randomIndex, out Func<IMiniGameViewController> miniGameStartAction))
                throw new KeyNotFoundException("Unknown mini game type");

            _miniGameDisposable = SubscribeController(miniGameStartAction());
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
            _walletService.Coins.Subtract(_earnedCoinsBeforeMiniGame / 2);
            ResetMiniGame();
        }

        private void OnMiniGameComplete()
        {
            ResetMiniGame();
        }

        private void ResetMiniGame()
        {
            _countTapsToStartMiniGame = GetUpdateTapsToStartMiniGame();
            _earnedCoinsBeforeMiniGame = 0;
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

        private enum MiniGameType
        {
            Password = 0,
        }
    }
}