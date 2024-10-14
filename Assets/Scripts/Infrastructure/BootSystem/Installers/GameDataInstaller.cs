using Infrastructure.Data.Effects;
using Infrastructure.Data.Game;
using Infrastructure.Data.Game.Shop;
using UnityEngine;
using Zenject;

namespace Infrastructure.BootSystem.Installers
{
    public class GameDataInstaller : MonoInstaller
    {
        [SerializeField] private BuildingsData _buildingsData;
        [SerializeField] private CoinsSpawnerData _coinsSpawnerData;
        [SerializeField] private ShopData _shopData;
        
        public override void InstallBindings()
        {
            Container.Bind<BuildingsData>().FromInstance(_buildingsData).AsSingle();
            Container.Bind<CoinsSpawnerData>().FromInstance(_coinsSpawnerData).AsSingle();
            Container.Bind<ShopData>().FromInstance(_shopData).AsSingle();
        }
    }
}