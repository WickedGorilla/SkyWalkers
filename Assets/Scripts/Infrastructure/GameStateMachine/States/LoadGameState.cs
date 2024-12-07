using System.Collections.Generic;
using Game.BuildingSystem;
using Game.Environment;
using Game.Invite;
using Game.Player;
using Game.UpdateResponseServices;
using Infrastructure.Network;
using Infrastructure.Network.Request;
using Infrastructure.Network.Request.Base.Player;
using Infrastructure.SceneManagement;
using Infrastructure.Telegram;
using Newtonsoft.Json;
using SkyExtensions.Awaitable;
using UI.Core;
using UI.Views;
using UI.Views.EveryDayPopup;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Infrastructure
{
    public class LoadGameState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IServerRequestSender _serverRequestSender;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ViewService _viewService;
        private readonly EnvironmentHolder _environmentHolder;
        private readonly PlayerHolder _playerHolder; 
        private readonly PlayerMovementByTap _playerMovementByTap;
        private readonly BuildingMovementSystem _buildingMovementSystem;
        private readonly TelegramLauncher _telegramLauncher;
        private readonly InviteSystem _inviteSystem;
        private readonly List<IResponseHandler> _responseHandlers;

        private const string SceneName = "Game";

        public LoadGameState(SceneLoader sceneLoader,
            IServerRequestSender serverRequestSender,
            LoadingCurtain loadingCurtain,
            IGameStateMachine gameStateMachine,
            ViewService viewService,
            EnvironmentHolder environmentHolder,
            PlayerHolder playerHolder,
            PlayerMovementByTap playerMovementByTap,
            BuildingMovementSystem buildingMovementSystem,
            TelegramLauncher telegramLauncher,
            InviteSystem inviteSystem,
            List<IResponseHandler> responseHandlers)
        {
            _sceneLoader = sceneLoader;
            _serverRequestSender = serverRequestSender;
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
            _viewService = viewService;
            _environmentHolder = environmentHolder;
            _playerHolder = playerHolder;
            _playerMovementByTap = playerMovementByTap;
            _buildingMovementSystem = buildingMovementSystem;
            _telegramLauncher = telegramLauncher;
            _inviteSystem = inviteSystem;
            _responseHandlers = responseHandlers;
        }

        public async void Enter()
        {
            await _sceneLoader.LoadSceneAsync(SceneName);
            await WaitLoadTelegramInfo();

            RegisterServerHandlers();

            _serverRequestSender.Initialize(_telegramLauncher.UserId);
            
            ServerResponse<GameData> response =
                await _serverRequestSender.SendToServerAndHandle<LoginRequest, GameData>(
                    GetLoginRequest(),
                    ServerAddress.Login, OnErrorLogin);

            if (!response.Success)
            {
                Debug.LogError("Failed to load game state");
                return;
            }

            var data = response.Data;
            _serverRequestSender.UpdateToken(data.Token);

            InitializeScene();
            InitializePlayer();
            _inviteSystem.Initialize(data.ReferralInfo);

            ShowPopups(data);
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void ShowPopups(GameData data)
        {
            if (data.BalanceUpdate.Coins == 0) 
                _viewService.AddPopupToQueueAndShow<StartScreenView, StartScreenViewController>();

            if (data.AutoTapCoins > 0)
            {
                _viewService.AddPopupToQueueAndShow<AutoTapClaimView, AutoTapClaimViewController>(controller =>
                    controller.SetInfo(data.AutoTapCoins));
            }
               
            if (data.ClaimBonus) 
                _viewService.AddPopupToQueueAndShow<EveryDayBonusView, EveryDayBonusViewController>();

            if (data.ReferralInfo is { CountReferrals: > 0, NewAddedValue: > 0 })
            {
                _viewService.AddPopupToQueueAndShow<ReferralsUpdatePopup, ReferralsUpdatePopupController>(controller 
                    => controller.SetInfo(data.ReferralInfo));
            }
        }
        
        private void RegisterServerHandlers()
        {
            foreach (var handler in _responseHandlers)
                handler.StartListening();
        }

        public void Exit()
            => _loadingCurtain.ShowStartButton();

        private async Awaitable WaitLoadTelegramInfo()
            => await AwaitableExtensions.WaitUntilAsync(() => _telegramLauncher.IsInit);

        private void InitializeScene()
        {
            var environment = Object.FindAnyObjectByType<EnvironmentObjects>();
            _environmentHolder.Hold(environment);
            _playerHolder.Hold(environment.Player);

            _viewService.CreateRoot();
            _buildingMovementSystem.Initialize();
        }

        private void InitializePlayer()
            => _playerMovementByTap.Initialize();

        private LoginRequest GetLoginRequest()
        {
            var tgData = _telegramLauncher.TgData;

            return new LoginRequest
            {
                ReferralId = _telegramLauncher.ReferralCode,
                UserId = tgData.id,
                InitData = tgData.InitData,
                UserName = tgData.username
            };
        }

        private void OnErrorLogin(long errorCode, string data)
        {
            if (errorCode == 401)
            {
                var result = JsonConvert.DeserializeObject<LoginErrorResponse>(data);
                _loadingCurtain.ShowLog(result.Message);
            }
        }
    }
}