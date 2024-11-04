using System.Collections.Generic;
using Game.Validation.ValidationActions;
using Infrastructure.Network;
using Player;
using UnityEngine;

namespace Game.Validation
{
    public class CoinValidationService
    {
        private readonly WalletService _walletService;
        private readonly IServerRequestSender _serverRequestSender;
        private readonly LinkedList<IValidationAction> _stackActions = new();

        private float _nextTimeUpdate;

        private const float TimeIntervalUpdate = 10f;
        
        public CoinValidationService(WalletService walletService, IServerRequestSender serverRequestSender)
        {
            _walletService = walletService;
            _serverRequestSender = serverRequestSender;
        }

        public void Start()
        {
            _walletService.Coins.OnChangeValue += OnChangeCoins;
            _walletService.PlayPass.OnChangeValue += OnPlayPassActivated;
            _walletService.Boosts.OnChangeValue += OnBoostActivated;
        }

        public void Stop()
        {
            _walletService.Coins.OnChangeValue -= OnChangeCoins;
            _walletService.PlayPass.OnChangeValue -= OnPlayPassActivated;
            _walletService.Boosts.OnChangeValue -= OnBoostActivated;
        }

        private void OnChangeCoins(int coins)
        {
            int energy = _walletService.Energy.Count;
            var action = new TapCoinValidationAction(coins, energy);
            
            if (_stackActions.Count == 0 || _stackActions.Last.Value is not TapCoinValidationAction)
            {
                _stackActions.AddLast(action);
            }
            else
                _stackActions.Last.Value = action;
        }

        private void OnPlayPassActivated(int newCount)
        {
            var action = new ActivatePlayPassAction();
            _stackActions.AddLast(action);
        }

        private void OnBoostActivated(int newCount)
        {
            var action = new ActivateBoostAction();
            _stackActions.AddLast(action);
        }

        private void SendValidationRequest()
        {
            if (Time.time < _nextTimeUpdate || _stackActions.Count == 0)
                return;

            _nextTimeUpdate = Time.time + TimeIntervalUpdate;

            _serverRequestSender.SendToServer<ValidationCoinsRequest, ValidationCoinsResponse>();

            _stackActions.Clear();
        }
    }
}