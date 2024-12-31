using System;
using Game.BuildingSystem;
using Game.Wallet;
using Player;
using UI.Core;
using UI.Hud;
using UI.Views;
using UnityEngine;

namespace Game.Player
{
    public class FarmCoinsSystem
    {
        private readonly IPlayerHolder _playerHolder;
        private readonly BuildingMovementSystem _buildingMovementSystem;
        private readonly WalletService _walletService;
        private readonly CoinsCalculatorService _coinsCalculatorService;
        private readonly ClickCoinSpawner _clickCoinSpawner;
        private readonly BoostSystem _boostSystem;
        private readonly ViewService _viewService;

        private IDisposable _buildingMovement;

        public event Action<int> OnFarmCoinsPerTap;
        
        public FarmCoinsSystem(IPlayerHolder playerHolder,
            BuildingMovementSystem buildingMovementSystem,
            WalletService walletService,
            CoinsCalculatorService coinsCalculatorService,
            ClickCoinSpawner clickCoinSpawner,
            BoostSystem boostSystem,
            ViewService viewService)
        {
            _playerHolder = playerHolder;
            _buildingMovementSystem = buildingMovementSystem;
            _walletService = walletService;
            _coinsCalculatorService = coinsCalculatorService;
            _clickCoinSpawner = clickCoinSpawner;
            _boostSystem = boostSystem;
            _viewService = viewService;
        }

        public void Initialize()
        {
            _clickCoinSpawner.Initialize();
        }

        public void Subscribe()
        {
            _buildingMovementSystem.Subscribe();
            _buildingMovement = _playerHolder.Player.AddClimbListener(_buildingMovementSystem.DoMove);
        }

        public void Unsubscribe()
        {
            _buildingMovementSystem.UnSubscribe();
            _buildingMovement.Dispose();
        }

        public void Tap(Vector2 tapPosition)
        {
            int calculateCoins = _coinsCalculatorService.CalculateCoinsByTap();
            var energy = ConvertCoinsToAvialableEnergy(calculateCoins);
            
            if (energy == 0)
                return;

            _clickCoinSpawner.SpawnCoinEffect(energy, tapPosition);
            _walletService.Coins.Add(energy);
            _playerHolder.Player.AnimateByClick();
            
            OnFarmCoinsPerTap?.Invoke(energy);
        }

        private int ConvertCoinsToAvialableEnergy(int coins)
        {
            if (_boostSystem.IsBoost)
                return coins;
            
            if (_walletService.Energy.Count == 0)
            {
                if (!_boostSystem.UsePlayPass())
                    _viewService.AddPopupToQueueAndShow<NoEnergyPopup, NoEnergyPopupController>();

                return 0;
            }

            if (_walletService.Energy.Count < coins)
                coins = _walletService.Energy.Count;

            _walletService.Energy.Subtract(coins);
            return coins;
        }
    }
}