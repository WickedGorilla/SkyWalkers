using Game.BuildingSystem;
using Game.Environment;
using Game.Player;
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
#if TEST
            _container.Bind<IServerRequestSender>().To<TestServerRequestSender>().AsSingle(); 
#else
            _container.Bind<IServerRequestSender>().To<ServerRequestSender>().AsSingle();
#endif
 
        }

        private void BindPlayer()
        {
            _container.Bind<WalletService>().AsSingle();
            _container.Bind<BuildingMovementSystem>().AsSingle();
            _container.Bind<PlayerMovementByTap>().AsSingle();
            _container.BindInterfacesAndSelfTo<PlayerHolder>().AsSingle();
            _container.BindInterfacesAndSelfTo<ClickCoinSpawner>().AsSingle();
            _container.BindInterfacesAndSelfTo<CoinsCalculatorService>().AsSingle();
            _container.BindInterfacesAndSelfTo<BonusSystem>().AsSingle();
        }
        
        private void BindEnvironment()
        {
            _container.BindInterfacesAndSelfTo<EnvironmentHolder>().AsSingle();
        }
    }
}