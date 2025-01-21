using Infrastructure.Data.Effects;
using Infrastructure.Data.Game;
using Infrastructure.Data.Game.MiniGames;
using Infrastructure.Data.Game.MiniGames.RainMiniGame;
using Infrastructure.Data.Game.MiniGames.SecurityGuardMiniGame;
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
        [SerializeField] private MiniGamesData _miniGamesData;
        
        public override void InstallBindings()
        {
            Container.Bind<BuildingsData>().FromInstance(_buildingsData).AsSingle();
            Container.Bind<CoinsSpawnerData>().FromInstance(_coinsSpawnerData).AsSingle();
            Container.Bind<ShopData>().FromInstance(_shopData).AsSingle();
            
            InstallMiniGamesData();
        }
        
        private void InstallMiniGamesData()
        {
            Container.Bind<MiniGamesData>().FromInstance(_miniGamesData).AsSingle();
            Container.Bind<PasswordMiniGameData>().FromInstance(_miniGamesData.PasswordMiniGame).AsSingle();
            Container.Bind<SecurityGuardMiniGameData>().FromInstance(_miniGamesData.SecurityGuardMiniGame).AsSingle();
            Container.Bind<RainMiniGameData>().FromInstance(_miniGamesData.RainMiniGameData).AsSingle();
        }
    }
}