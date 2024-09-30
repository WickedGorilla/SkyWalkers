using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Core
{
    public class ViewFabric
    {
        private readonly DiContainer _diContainer;
        private readonly ViewPrefabsData _viewPrefabsData;
        private readonly Dictionary<Type, View> _views;
        
        public ViewFabric(DiContainer diContainer, 
            ViewPrefabsData viewPrefabsData)
        {
            _diContainer = diContainer;
            _viewPrefabsData = viewPrefabsData;
            _views = viewPrefabsData.Prefabs
                .ToDictionary(x => x.GetType(), x => x);
        }

        public TController Create<TView, TController>(Transform root) where TView : View where TController : ViewController<TView>
        {
            if (!_views.TryGetValue(typeof(TView), out View view))
            {
                Debug.LogError($"Not Found in loaded view {typeof(TView)}");
                return null;
            }
            
            TView viewPrefab = view as TView;
            return InstantiateView<TView, TController>(viewPrefab, root);
        }
        
        private TController InstantiateView<TView, TController>(TView viewPrefab, Transform root) where TView : View where TController : ViewController<TView>
        {
            TView view = Object.Instantiate(viewPrefab, root);
            TController viewController = _diContainer.Instantiate<TController>(new []{view});
            return viewController;
        }

        public UIRoot CreateRoot() 
            => Object.Instantiate(_viewPrefabsData.Root);
    }
}