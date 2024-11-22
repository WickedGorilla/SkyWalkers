using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.BuildingSystem;
using Game.Environment;
using Game.Invite;
using Game.Perks;
using Game.Player;
using Game.UpdateResponseServices;
using Infrastructure.Network;
using Infrastructure.Network.Request;
using Infrastructure.Network.Request.Base.Player;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.RequestHandler;
using Infrastructure.SceneManagement;
using Infrastructure.Telegram;
using Newtonsoft.Json;
using Player;
using UI.Core;
using UnityEngine;
using static System.String;
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

            _gameStateMachine.Enter<MainMenuState>();

            if (data.BalanceUpdate.Coins <= 100)
                ShowStartsScreen();

            if (data.AutoTapCoins > 0)
                ShowAutoTapClaimScreen(data.AutoTapCoins);
        }

        private void RegisterServerHandlers()
        {
            foreach (var handler in _responseHandlers)
                handler.StartListening();
        }

        public void Exit()
            => _loadingCurtain.ShowStartButton();

        private async UniTask WaitLoadTelegramInfo()
            => await UniTask.WaitUntil(() => _telegramLauncher.IsInit);

        private void InitializeScene()
        {
            var environment = Object.FindObjectOfType<EnvironmentObjects>();
            _environmentHolder.Hold(environment);
            _playerHolder.Hold(environment.Player);

            _viewService.CreateRoot();
            _buildingMovementSystem.Initialize();
        }

        private void InitializePlayer()
            => _playerMovementByTap.Initialize();

        private LoginRequest GetLoginRequest()
        {
#if UNITY_EDITOR
            return new LoginRequest(
                "",
                "lol", "", "wickedgorilla");
#endif
            var tgData = _telegramLauncher.TgData;
            
            return new LoginRequest
            {
                AuthDate = tgData.auth_date.ToString(),
                FirstName = tgData.first_name,
                Hash = tgData.hash,
                LastName = tgData.last_name,
                PhotoUrl = tgData.photo_url,
                ReferralId = _telegramLauncher.ReferralCode,
                UserId = tgData.id,
                UserName = tgData.username
            };
        }

        private void ShowStartsScreen()
            => _viewService.ShowPermanent<StartScreenView, StartScreenViewController>();

        private void ShowAutoTapClaimScreen(int claimCoins)
            => _viewService.ShowPermanent<AutoTapClaimView, AutoTapClaimViewController>().SetInfo(claimCoins);
        
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