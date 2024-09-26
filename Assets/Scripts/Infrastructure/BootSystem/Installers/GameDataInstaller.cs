using Infrastructure.Data.Effects;
using Infrastructure.Data.Game;
using UnityEngine;
using Zenject;

namespace Infrastructure.BootSystem.Installers
{
    public class GameDataInstaller : MonoInstaller
    {
        [SerializeField] private BuildingsData _buildingsData;
        [SerializeField] private CoinsSpawnerData _coinsSpawnerData;
        
        public override void InstallBindings()
        {
            Container.Bind<BuildingsData>().FromInstance(_buildingsData).AsSingle();
            Container.Bind<CoinsSpawnerData>().FromInstance(_coinsSpawnerData).AsSingle();
        }
    }
}