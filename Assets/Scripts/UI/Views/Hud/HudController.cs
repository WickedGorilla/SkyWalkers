using Game.Environment;
using Game.Infrastructure;
using Game.Player;
using Player;
using UI.Core;
using UI.Views;
using UnityEngine;

namespace UI.Hud
{
    public class HudController : ViewController<HudView>
    {
        private readonly PlayerMovementByTap _playerMovementByTap;
        private readonly WalletService _walletService;
        private readonly BonusSystem _bonusSystem;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ViewService _viewService;
        private readonly IEnvironmentHolder _environmentHolder;

        public HudController(HudView view,
            PlayerMovementByTap playerMovementByTap,
            WalletService walletService,
            BonusSystem bonusSystem,
            IGameStateMachine gameStateMachine,
            ViewService viewService,
            IEnvironmentHolder environmentHolder) : base(view)
        {
            _playerMovementByTap = playerMovementByTap;
            _walletService = walletService;
            _bonusSystem = bonusSystem;
            _gameStateMachine = gameStateMachine;
            _viewService = viewService;
            _environmentHolder = environmentHolder;
        }

        protected override void OnShow()
        {
            View.TouchArea.OnClicked += OnTap;
            View.EnergyButton.onClick.AddListener(OnClickEnergy);
            View.BoostButton.onClick.AddListener(OnClickBoost);
            View.BackButton.onClick.AddListener(OnClickBack);
            View.GearButton.onClick.AddListener(OnClickGear);

            _walletService.Coins.OnChangeValue += View.SetCoinsCount;
            _walletService.Energy.OnChangeValue += SetEnergy;

            View.Initialize(_walletService.Coins, _walletService.Energy, _walletService.PlayPass, _walletService.Boosts);
        }

        protected override void OnHide()
        {
            View.TouchArea.OnClicked -= OnTap;
            View.EnergyButton.onClick.RemoveListener(OnClickEnergy);
            View.BoostButton.onClick.RemoveListener(OnClickBoost);
            View.BackButton.onClick.RemoveListener(OnClickBack);
            View.GearButton.onClick.RemoveListener(OnClickGear);

            _walletService.Coins.OnChangeValue -= View.SetCoinsCount;
            _walletService.Energy.OnChangeValue -= SetEnergy;
        }

        private void OnTap(Vector2 position)
            => _playerMovementByTap.Tap(position);

        private void SetEnergy(int count)
            => View.FillEnergy(count, _walletService.Energy.Max, _walletService.PlayPass);

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
            View.SetBoostCount(_walletService.Boosts);
            return;

            void OnComplete() 
                => View.EnableBoost(false);
        }
        
        private void OnClickBack()
        {
            if (_environmentHolder.Environment.Animated)
                return;
            
            _gameStateMachine.Enter<MainMenuState>();
        }
        
        private void OnClickGear() 
            => _viewService.Show<ShopInGameView, ShopInGameController>();
    }
}