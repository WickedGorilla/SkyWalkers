using Game.Player;
using Player;
using UI.Core;
using UnityEngine;

namespace UI.Hud
{
    public class HudController : ViewController<HudView>
    {
        private readonly PlayerMovementByTap _playerMovementByTap;
        private readonly WalletService _walletService;
        private readonly BonusSystem _bonusSystem;

        public HudController(HudView view,
            PlayerMovementByTap playerMovementByTap,
            WalletService walletService,
            BonusSystem bonusSystem) : base(view)
        {
            _playerMovementByTap = playerMovementByTap;
            _walletService = walletService;
            _bonusSystem = bonusSystem;
        }

        protected override void OnShow()
        {
            View.TouchArea.OnClicked += OnTap;
            View.EnergyButton.onClick.AddListener(OnClickEnergy);
            View.BoostButton.onClick.AddListener(OnClickBoost);

            _walletService.Coins.OnChangeValue += View.SetCoinsCount;
            _walletService.Energy.OnChangeValue += SetEnergy;

            View.Initialize(_walletService.Coins, _walletService.Energy, _walletService.EnergyFlash);
        }

        protected override void OnHide()
        {
            View.TouchArea.OnClicked -= OnTap;
            View.EnergyButton.onClick.RemoveListener(OnClickEnergy);
            View.BoostButton.onClick.RemoveListener(OnClickBoost);

            _walletService.Coins.OnChangeValue -= View.SetCoinsCount;
            _walletService.Energy.OnChangeValue -= SetEnergy;
        }

        private void OnTap(Vector2 position)
            => _playerMovementByTap.Tap(position);

        private void SetEnergy(int count)
            => View.FillEnergy(count, _walletService.Energy.Max, _walletService.EnergyFlash);

        private void OnClickEnergy()
        {
            if (!_bonusSystem.UseEnergy())
                return;
        }

        private void OnClickBoost()
        {
            if (!_bonusSystem.UseBoost(View.UpdateTimerText, OnComplete))
                return;
            
            View.EnableBoost(true);
            return;

            void OnComplete() 
                => View.EnableBoost(false);
        }
        
    }
}