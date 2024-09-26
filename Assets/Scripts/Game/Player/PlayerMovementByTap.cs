using System;
using Game.BuildingSystem;
using Game.Wallet;
using Player;
using UI.Hud;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementByTap
    {
        private readonly IPlayerHolder _playerHolder;
        private readonly BuildingMovementSystem _buildingMovementSystem;
        private readonly WalletService _walletService;
        private readonly CoinsCalculatorService _coinsCalculatorService;
        private readonly ClickCoinSpawner _clickCoinSpawner;

        private IDisposable _buildingMovement;

        public PlayerMovementByTap(IPlayerHolder playerHolder,
            BuildingMovementSystem buildingMovementSystem,
            WalletService walletService,
            CoinsCalculatorService coinsCalculatorService,
            ClickCoinSpawner clickCoinSpawner)
        {
            _playerHolder = playerHolder;
            _buildingMovementSystem = buildingMovementSystem;
            _walletService = walletService; 
            _coinsCalculatorService = coinsCalculatorService;
            _clickCoinSpawner = clickCoinSpawner;
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
            int count = _coinsCalculatorService.CalculateCoinsByTap();
            _clickCoinSpawner.SpawnCoinEffect(count, tapPosition);
            
            _walletService.Coins.Collect(count);
            _playerHolder.Player.AnimateByClick();
        }
    }
}