using Game.BuildingSystem;
using Game.Environment;
using Game.Player;
using Infrastructure.Network;
using Infrastructure.Network.Request;
using Infrastructure.Network.Response;
using Infrastructure.SceneManagement;
using Player;
using UI.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class LoadGameState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IServerRequestSender _serverRequestSender;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly WalletService _balanceService;
        private readonly ViewService _viewService;
        private readonly EnvironmentHolder _environmentHolder;
        private readonly PlayerHolder _playerHolder;
        private readonly PlayerMovementByTap _playerMovementByTap;
        private readonly BuildingMovementSystem _buildingMovementSystem;

        private const string SceneName = "Game";

        public LoadGameState(SceneLoader sceneLoader,
            IServerRequestSender serverRequestSender,
            LoadingCurtain loadingCurtain,
            IGameStateMachine gameStateMachine,
            WalletService balanceService,
            ViewService viewService,
            EnvironmentHolder environmentHolder,
            PlayerHolder playerHolder,
            PlayerMovementByTap playerMovementByTap,
            BuildingMovementSystem buildingMovementSystem)
        {
            _sceneLoader = sceneLoader;
            _serverRequestSender = serverRequestSender;
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
            _balanceService = balanceService;
            _viewService = viewService;
            _environmentHolder = environmentHolder;
            _playerHolder = playerHolder;
            _playerMovementByTap = playerMovementByTap;
            _buildingMovementSystem = buildingMovementSystem;
        }

        public async void Enter()
        {
            await _sceneLoader.LoadSceneAsync(SceneName);

            var response = await _serverRequestSender.SendToServer<LoginRequest, PlayerData>(new LoginRequest("3212"),
                ServerPath.Login);

            if (!response.Success)
            {
                Debug.LogError("Failed to load game state"); 
                return;
            }

            InitializeData(response.Data);

            InitializeScene();
            InitializePlayer();

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
            => _loadingCurtain.ShowStartButton();

        private void InitializeData(PlayerData data)
            => _balanceService.Initialize(data);

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
    }
}