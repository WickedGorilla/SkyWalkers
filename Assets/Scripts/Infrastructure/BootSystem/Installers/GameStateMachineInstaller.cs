using Game.Infrastructure;
using Infrastructure.SceneManagement;
using UnityEngine;
using Zenject;

namespace Infrastructure.BootSystem.Installers
{
    public class GameStateMachineInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;

        public override void InstallBindings()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<LoadingCurtain>().FromInstance(Instantiate(_loadingCurtain)).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}