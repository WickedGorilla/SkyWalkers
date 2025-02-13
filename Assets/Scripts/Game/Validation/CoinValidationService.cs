using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.MiniGames;
using Game.Player;
using Infrastructure.Network;
using Player;
using UnityEngine;

namespace Game.Validation
{
    public class CoinValidationService
    {
        private readonly WalletService _walletService;
        private readonly IServerRequestSender _serverRequestSender;
        private readonly BoostSystem _boostSystem;
        private readonly FarmCoinsSystem _farmCoinsSystem;
        private readonly MiniGamesSystem _miniGamesSystem;
        private readonly LinkedList<IPlayerActionData> _stackActions = new();

        private float _nextTimeUpdate;
        private int _lastUpdateBalance;
        private bool _boostState;

        private const float TimeIntervalUpdate = 5f;

        private CancellationTokenSource _cancellationTokenSource;

        public CoinValidationService(WalletService walletService,
            IServerRequestSender serverRequestSender,
            BoostSystem boostSystem,
            FarmCoinsSystem farmCoinsSystem,
            MiniGamesSystem miniGamesSystem)
        {
            _walletService = walletService;
            _serverRequestSender = serverRequestSender;
            _boostSystem = boostSystem;
            _farmCoinsSystem = farmCoinsSystem;
            _miniGamesSystem = miniGamesSystem;
        }

        public void Start()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap += OnChangeCoins;

            _boostSystem.OnUseBoost += OnBoostActivated;
            _boostSystem.OnEndBoost += OnBoostEnd;

            _boostSystem.OnUsePlayPass += OnPlayPassActivated;

            _miniGamesSystem.OnEnterMiniGame += OnEnterMiniGame;
            _miniGamesSystem.OnCompleteMiniGame += OnEndMiniGame;

            StartTimerValidation();
        }

        public void Stop()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap -= OnChangeCoins;

            _boostSystem.OnUseBoost -= OnBoostActivated;
            _boostSystem.OnEndBoost -= OnBoostEnd;

            _boostSystem.OnUsePlayPass -= OnPlayPassActivated;

            _miniGamesSystem.OnEnterMiniGame -= OnEnterMiniGame;
            _miniGamesSystem.OnCompleteMiniGame -= OnEndMiniGame;

            _cancellationTokenSource.Cancel();
            SendValidationRequest();
        }

        private void StartTimerValidation()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            SendTask(_cancellationTokenSource);
        }

        private void StopTimerValidation()
        {
            _cancellationTokenSource.Cancel();
            SendValidationRequest();
        }

        private async void SendTask(CancellationTokenSource token)
        {
            _nextTimeUpdate = Time.time + TimeIntervalUpdate;
            _lastUpdateBalance = _walletService.Coins.Count;

            while (!token.IsCancellationRequested)
            {
                await UniTask.NextFrame();

                if (Time.time < _nextTimeUpdate || _stackActions.Count == 0)
                    continue;

                SendValidationRequest();
                _nextTimeUpdate = Time.time + TimeIntervalUpdate;
            }
        }

        private void OnChangeCoins(int farmedCoins)
        {
            var coins = _walletService.Coins.Count;
            var tapedCoins = coins - _lastUpdateBalance;
            _lastUpdateBalance = coins;

            if (!_boostSystem.IsBoost)
                AddToStack(new CoinPlayerActionData(tapedCoins, _walletService.Energy.Count));
            else
                AddToStack(new CoinWithBoostActionData(tapedCoins));
        }

        private void AddToStack<TCoinValidation>(TCoinValidation action) where TCoinValidation : ICoinPlayerActionData
        {
            if (_stackActions.Count > 0 && _stackActions.Last.Value is TCoinValidation coinValidationData)
            {
                var data = coinValidationData;
                data.CoinsCount += action.CoinsCount;
                _stackActions.Last.Value = data;
            }
            else
                _stackActions.AddLast(action);
        }

        private void OnPlayPassActivated()
            => _stackActions.AddLast(new ActivatePlayPassActionData());

        private void OnBoostActivated()
        {
            _stackActions.AddLast(new ActivateBoostActionData());
            StopTimerValidation();
        }

        private void OnBoostEnd()
        {
            _stackActions.AddLast(new BoostEndActionData());
            SendValidationRequest();
            StartTimerValidation();
        }

        private void OnEnterMiniGame(MiniGameType miniGame)
        {
            _stackActions.AddLast(new EnterMiniGameActionData(miniGame, _miniGamesSystem.EarnedCoinsBeforeMiniGame));
            StopTimerValidation();
        }

        private void OnEndMiniGame(bool isComplete)
        {
            _stackActions.AddLast(new EndMiniGameActionData(isComplete));
            SendValidationRequest();
            StartTimerValidation();
        }

        private void SendValidationRequest()
        {
            if (_stackActions.Count == 0)
                return;

            _lastUpdateBalance = _walletService.Coins.Count;

#if DEV_BUILD
            Debug.Log("Sending validation request");
#endif

            var message = new ValidationCoinsRequest(_stackActions.ToArray());

            _serverRequestSender
                .SendToServerAndHandle<ValidationCoinsRequest, ValidationCoinsResponse>(message, ServerAddress.TapCoinsValidation);

            _stackActions.Clear();
        }

     
    }
}