using Game.BuildingSystem;
using Game.Environment;
using Game.Invite;
using Game.Perks;
using Game.Player;
using Game.Validation;
using Game.Wallet;
using Infrastructure.Network;
using Player;
using UI.Hud;
using Zenject;

namespace Game.Infrastructure
{
    public class BindGameState : IState
    {
        private readonly DiContainer _container;
        private readonly IGameStateMachine _gameStateMachine;

        public BindGameState(DiContainer container,
            IGameStateMachine gameStateMachine)
        {
            _container = container;
            _gameStateMachine = gameStateMachine;
        } 
        
        public void Enter()
        {
            BindNetworkService();
            BindEnvironment();
            BindPlayer();
            
            _gameStateMachine.Enter<LoadGameState>();
        }

        public void Exit()
        {
            
        }

        private void BindNetworkService()
        {
/*#if TEST
            _container.Bind<IServerRequestSender>().To<FakeServerRequestSender>().AsSingle();
#else*/
            _container.Bind<IServerRequestSender>().To<ServerRequestSender>().AsSingle();
        }

        private void BindPlayer()
        {
            _container.Bind<WalletService>().AsSingle();
            _container.Bind<BuildingMovementSystem>().AsSingle();
            _container.Bind<PlayerMovementByTap>().AsSingle();
            _container.Bind<PerksService>().AsSingle();
            _container.Bind<CoinValidationService>().AsSingle();
            _container.Bind<InviteSystem>().AsSingle();
            
            _container.BindInterfacesAndSelfTo<PlayerHolder>().AsSingle();
            _container.BindInterfacesAndSelfTo<ClickCoinSpawner>().AsSingle();
            _container.BindInterfacesAndSelfTo<CoinsCalculatorService>().AsSingle();
            _container.BindInterfacesAndSelfTo<BoostSystem>().AsSingle();
        }
        
        private void BindEnvironment()
        {
            _container.BindInterfacesAndSelfTo<EnvironmentHolder>().AsSingle();
        }
    }
}