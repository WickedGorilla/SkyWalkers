using System;
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
        private readonly FarmCoinsSystem _farmCoinsSystem;
        private readonly WalletService _walletService;
        private readonly BoostSystem _boostSystem;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ViewService _viewService;
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly ClickCoinSpawner _clickCoinSpawner;

        public HudController(HudView view,
            FarmCoinsSystem farmCoinsSystem,
            WalletService walletService,
            BoostSystem boostSystem,
            IGameStateMachine gameStateMachine,
            ViewService viewService,
            IEnvironmentHolder environmentHolder,
            ClickCoinSpawner clickCoinSpawner) : base(view)
        {
            _farmCoinsSystem = farmCoinsSystem;
            _walletService = walletService;
            _boostSystem = boostSystem;
            _gameStateMachine = gameStateMachine;
            _viewService = viewService;
            _environmentHolder = environmentHolder;
            _clickCoinSpawner = clickCoinSpawner;
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

            View.Initialize(_walletService.Coins, _walletService.Energy, _walletService.PlayPass,
                _walletService.Boosts);
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

        public void SpawnSubtractText(int subtractCoins)
        {
            var startPosition = View.CoinsText.transform.position - new Vector3(0f, 300f);
            var endPosition = startPosition + new Vector3(0f, 150f);
            _clickCoinSpawner.SpawnText($"-{subtractCoins}", startPosition, endPosition, 2f);
        }

        private void OnTap(Vector2 position)
            => _farmCoinsSystem.Tap(position);

        private void SetEnergy(int count)
            => View.FillEnergy(count, _walletService.Energy.Max, _walletService.PlayPass);

        private void OnClickEnergy()
        {
            if (_boostSystem.UsePlayPass())
                return;

            _viewService.AddPopupToQueueAndShow<NoEnergyPopup, NoEnergyPopupController>();
        }

        private void OnClickBoost()
        {
            if (!_boostSystem.UseBoost(StartTimer))
            {
                _viewService.AddPopupToQueueAndShow<NoEnergyPopup, NoEnergyPopupController>();
                return;
            }

            View.EnableBoost(true);
            View.SetBoostCount(_walletService.Boosts);
        }

        private void StartTimer(int time, Action onComplete)
        {
            var timer = View.Timer.CreateTimer(time, OnComplete);
            timer.Start();

            void OnComplete()
            {
                onComplete();
                View.EnableBoost(false);
            }
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