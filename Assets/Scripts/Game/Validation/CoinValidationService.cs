using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Game.Player;
using Infrastructure.Network;
using Infrastructure.Network.Response.Player;
using Newtonsoft.Json;
using Player;
using UnityEngine;
using static System.String;

namespace Game.Validation
{
    public class CoinValidationService
    {
        private readonly WalletService _walletService;
        private readonly IServerRequestSender _serverRequestSender;
        private readonly BoostSystem _boostSystem;
        private readonly FarmCoinsSystem _farmCoinsSystem;
        private readonly LinkedList<IPlayerActionData> _stackActions = new();

        private float _nextTimeUpdate;
        private float _lastTimeUpdate;
        private int _lastUpdateBalance;
        private bool _boostState;

        private const float TimeIntervalUpdate = 10f;
        private const long ConflictErrorCode = 409;

        private CancellationTokenSource _cancellationTokenSource;
        
        public CoinValidationService(WalletService walletService, 
            IServerRequestSender serverRequestSender, 
            BoostSystem boostSystem,
            FarmCoinsSystem farmCoinsSystem)
        {
            _walletService = walletService;
            _serverRequestSender = serverRequestSender;
            _boostSystem = boostSystem;
            _farmCoinsSystem = farmCoinsSystem;
        }
        
        public void Start()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap += OnChangeCoins;
            _boostSystem.OnUseBoost += OnBoostActivated;
            _boostSystem.OnEndBoost += OnBoostEnd;
            _boostSystem.OnUsePlayPass += OnPlayPassActivated;
            _boostSystem.OnUsePlayPass += OnPlayPassActivated;

            _cancellationTokenSource = new CancellationTokenSource();
            SendTask(_cancellationTokenSource);
        }

        public void Stop()
        {
            _farmCoinsSystem.OnFarmCoinsPerTap -= OnChangeCoins;
            _boostSystem.OnUseBoost -= OnBoostActivated;
            _boostSystem.OnEndBoost -= OnBoostEnd;
            _boostSystem.OnUsePlayPass -= OnPlayPassActivated;
            
            _cancellationTokenSource.Cancel();
            SendValidationRequest();
        }

        private async void SendTask(CancellationTokenSource token)
        {
            _nextTimeUpdate = Time.time + TimeIntervalUpdate;
            _lastUpdateBalance = _walletService.Coins.Count;
            
            while (true)
            {
                await Awaitable.NextFrameAsync(token.Token);

                if (token.IsCancellationRequested)
                    break;
                
                if (Time.time < _nextTimeUpdate || _stackActions.Count == 0)
                    continue;
                
                SendValidationRequest();
                _nextTimeUpdate =  Time.time + TimeIntervalUpdate;
            }
        }
        
        private void OnChangeCoins(int farmedCoins)
        {
            var coins = _walletService.Coins.Count;
            var tapedCoins = coins - _lastUpdateBalance;
            _lastUpdateBalance = coins;
             
            if (!_boostSystem.IsBoost)
            {
                AddToStack(new CoinPlayerActionData(tapedCoins, _walletService.Energy.Count));
            }
            else
            {
                AddToStack(new CoinWithBoostActionData(tapedCoins));
            }
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
        {
            var action = new ActivatePlayPassActionData();
            _stackActions.AddLast(action);
        }

        private void OnBoostActivated()
        {
            var action = new ActivateBoostActionData();
            _stackActions.AddLast(action);
        }

        private void OnBoostEnd()
        {
            var action = new BoostEndActionData();
            _stackActions.AddLast(action);
        }
        
        private void SendValidationRequest()
        {
            if (_stackActions.Count == 0 || _lastTimeUpdate + TimeIntervalUpdate / 3 > Time.time)
                return;

            _lastTimeUpdate = Time.time;
            _lastUpdateBalance = _walletService.Coins.Count;
            
            Debug.Log("Sending validation request");
            var message = new ValidationCoinsRequest(_stackActions.ToArray());
            _serverRequestSender.SendToServerAndHandle<ValidationCoinsRequest, ValidationCoinsResponse>(message, ServerAddress.TapCoinsValidation, OnValidationError);
            _stackActions.Clear();
        }

        private void OnValidationError(long code, string data)
        {
            Debug.LogError($"Response with error: {code}");

            if (data == Empty)
                return;
            
            switch (code)
            {
                case ConflictErrorCode:
                    _walletService.UpdateValues(JsonConvert.DeserializeObject<BalanceUpdate>(data));
                    _stackActions.Clear();
                    return;
            }
        }
    }
}