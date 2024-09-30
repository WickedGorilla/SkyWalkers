using Infrastructure.Data;
using UI.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.BootSystem.Installers
{
    public class ViewServiceInstaller : MonoInstaller
    {
        [SerializeField] private ViewPrefabsData _viewPrefabsData;
         
        public override void InstallBindings()
        {
            Container.Bind<ViewService>().AsSingle();
            Container.Bind<ViewFabric>().AsSingle();
            Container.Bind<ViewPrefabsData>().FromInstance(_viewPrefabsData).AsSingle();
        }
    }
}