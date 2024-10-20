using System;
using Cysharp.Threading.Tasks;
using Game.Environment;
using Game.Wallet;
using Player;

namespace Game.Player
{
    public class BonusSystem
    {
        private readonly WalletService _walletService;
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly CoinsCalculatorService _coinsCalculatorService;

        private const int BoostingTime = 30;
        
        public BonusSystem(WalletService walletService, 
            IEnvironmentHolder environmentHolder, 
            CoinsCalculatorService coinsCalculatorService)
        {
            _walletService = walletService;
            _environmentHolder = environmentHolder;
            _coinsCalculatorService = coinsCalculatorService;
        }

        public bool IsBoost { get; private set; }

        public bool UseEnergy()
        {
            if (_walletService.Energy.IsMax || !_walletService.EnergyFlash.Subtract(1))
                return false;

            _walletService.Energy.Add(_walletService.Energy.Max);
            return true;
        }

        public bool UseBoost(Action<int> onTickSecond, Action onComplete)
        {
            if (_walletService.Energy.Count == 0 || !_walletService.Boosts.Subtract(1))
                return false;

            Boosting(onTickSecond, onComplete);
            return true;
        }

        private async void Boosting(Action<int> onTickSecond, Action onComplete)
        {
            var environment = _environmentHolder.Environment;
            var coinsParticle = environment.CoinsParticle;
            coinsParticle.Play();
            environment.ShowSun();
            
            IsBoost = true;
            
            _coinsCalculatorService.UpdateInstruction(3);
            
            for (int i = BoostingTime; i > 0; i--)
            {
                onTickSecond(i);
                await UniTask.WaitForSeconds(1f);
            }
            
            onTickSecond(0);
            onComplete();
            
            coinsParticle.Stop();
            environment.HideSun();
            
            _coinsCalculatorService.ResetInstruction();
            IsBoost = false;
        }
        
    }
}