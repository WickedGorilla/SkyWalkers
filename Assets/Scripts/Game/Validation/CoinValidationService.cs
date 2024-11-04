using System.Collections.Generic;
using System.Linq;
using Game.Player;
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
        private readonly BoostSystem _boostSystem;
        private readonly LinkedList<IValidationAction> _stackActions = new();

        private float _nextTimeUpdate;
        private bool _boostState;

        private const float TimeIntervalUpdate = 10f;
        
        public CoinValidationService(WalletService walletService, 
            IServerRequestSender serverRequestSender, 
            BoostSystem boostSystem)
        {
            _walletService = walletService;
            _serverRequestSender = serverRequestSender;
            _boostSystem = boostSystem;
        }
        
        public void Start()
        {
            _walletService.Coins.OnChangeValue += OnChangeCoins;
            _boostSystem.OnUseBoost += OnBoostActivated;
            _boostSystem.OnEndBoost += OnBoostEnd;
            _boostSystem.OnUsePlayPass += OnPlayPassActivated;
        }

        public void Stop()
        {
            _walletService.Coins.OnChangeValue -= OnChangeCoins;
            _boostSystem.OnUseBoost -= OnBoostActivated;
            _boostSystem.OnEndBoost -= OnBoostEnd;
            _boostSystem.OnUsePlayPass -= OnPlayPassActivated;
        }

        private void OnChangeCoins(int coins)
        {
            if (_boostSystem.IsBoost)
            {
                AddToStack(new CoinValidationAction(coins, _walletService.Energy.Count));
            }
            else
            {
                AddToStack(new CoinWithBoostAction(coins));
            }
        }
        
        private void AddToStack<TCoinValidation>(TCoinValidation action) where TCoinValidation : ICoinValidationAction
        {
            if (_stackActions.Count == 0 || _stackActions.Last.Value is not TCoinValidation)
            {
                _stackActions.AddLast(action);
            }
            else
                _stackActions.Last.Value = action;
        }
        
        private void OnPlayPassActivated()
        {
            var action = new ActivatePlayPassAction();
            _stackActions.AddLast(action);
        }

        private void OnBoostActivated()
        {
            var action = new ActivateBoostAction();
            _stackActions.AddLast(action);
        }

        private void OnBoostEnd()
        {
            var action = new BoostEndAction();
            _stackActions.AddLast(action);
        }
        
        private void SendValidationRequest()
        {
            if (Time.time < _nextTimeUpdate || _stackActions.Count == 0)
                return;

            _nextTimeUpdate = Time.time + TimeIntervalUpdate;


            var message = new ValidationCoinsRequest(_stackActions.ToArray());
            _serverRequestSender
                .SendToServerAndHandle<ValidationCoinsRequest, ValidationCoinsResponse>(message, ServerAddress.TapCoinsValidation);

            _stackActions.Clear();
        }


    }
}