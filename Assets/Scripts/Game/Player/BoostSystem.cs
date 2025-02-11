using System;
using Game.Environment;
using Game.Validation;
using Game.Wallet;
using Player;
using UI.Views.Timer;
using UnityEngine;

namespace Game.Player
{
    public class BoostSystem
    {
        private readonly WalletService _walletService;
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly CoinsCalculatorService _coinsCalculatorService;

        private const int BoostingTime = 15;

        public BoostSystem(WalletService walletService,
            IEnvironmentHolder environmentHolder,
            CoinsCalculatorService coinsCalculatorService)
        {
            _walletService = walletService;
            _environmentHolder = environmentHolder;
            _coinsCalculatorService = coinsCalculatorService;
        }

        public event Action OnUseBoost;
        public event Action OnEndBoost;
        public event Action OnUsePlayPass;
        
        public bool IsBoost { get; private set; }

        public bool UsePlayPass()
        {
            if (_walletService.Energy.IsMax || !_walletService.PlayPass.Subtract(1))
                return false;

            _walletService.Energy.Add(_walletService.Energy.Max);
            OnUsePlayPass?.Invoke();
            return true;
        }

        public bool UseBoost(Action<int, Action> onCreateTimer)
        {
            if (_walletService.Boosts.Count == 0)
                return false;

            var energy = _coinsCalculatorService.CalculateCoinsByTap();

            if (_walletService.Energy.Count < energy)
            {
                if (_walletService.PlayPass.Count == 0)
                    return false;

                UsePlayPass();
            }
            
            _walletService.Energy.Subtract(energy);
            _walletService.Boosts.Subtract(1);
            onCreateTimer(BoostingTime, EndBoost);

            StartBoost();
            
            return true;
        }

        private void StartBoost()
        {
            OnUseBoost?.Invoke();
            var environment = _environmentHolder.Environment;
            environment.CoinsParticle.Play();
            environment.ShowSun();

            IsBoost = true;
            _coinsCalculatorService.UpdateInstruction(3);
        }
        
        private void EndBoost()
        {
            var environment = _environmentHolder.Environment;
            environment.CoinsParticle.Stop();
            environment.HideSun();

            _coinsCalculatorService.ResetInstruction();
            IsBoost = false;
            OnEndBoost?.Invoke();
        }
    }
}