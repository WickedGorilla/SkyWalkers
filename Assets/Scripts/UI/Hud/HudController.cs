using Game.Player;
using Player;
using UI.ViewService;
using UnityEngine;

namespace UI.Hud
{
    public class HudController : ViewController<HudView>
    {
        private readonly PlayerMovementByTap _playerMovementByTap;
        private readonly WalletService _walletService;

        public HudController(HudView view,
            PlayerMovementByTap playerMovementByTap,
            WalletService walletService) : base(view)
        {
            _playerMovementByTap = playerMovementByTap;
            _walletService = walletService;
        }
        
        protected override void OnShow()
        {
            View.TouchArea.OnClicked += OnTap;
            _walletService.Coins.OnChangeValue += View.SetCoinsCount;
            _walletService.Energy.OnChangeValue += SetEnergy;   
            
            View.Initialize(_walletService.Coins, _walletService.Energy, _walletService.EnergyFlash);
        }
        
        protected override void OnHide()
        {
            View.TouchArea.OnClicked -= OnTap;
            _walletService.Coins.OnChangeValue -= View.SetCoinsCount;
            _walletService.Energy.OnChangeValue -= SetEnergy;
        }

        private void OnTap(Vector2 position)
            => _playerMovementByTap.Tap(position);
        
        private void SetEnergy(int count) 
            => View.FillEnergy(count, _walletService.Energy.Max, _walletService.EnergyFlash);
    }
}