using System;
using System.Collections.Generic;

namespace UI.Core
{
    public class ViewService
    {
        private readonly ViewFabric _viewFabric;
        private readonly LinkedList<IViewController> _permanentViews;
        private readonly Dictionary<Type, IViewController> _createdViews;
        
        private IViewController _currentView;
        private UIRoot _root;

        public ViewService(ViewFabric viewFabric)
        {
            _viewFabric = viewFabric;
            _permanentViews = new LinkedList<IViewController>();
            _createdViews = new Dictionary<Type, IViewController>();
        }

        public TController Show<TView, TController>()
            where TView : View where TController : ViewController<TView>
        {
            _currentView?.Hide();
            var type = typeof(TView);
            
            if (!_createdViews.TryGetValue(type, out IViewController controller))
            {
                controller = _viewFabric.Create<TView, TController>(_root.Layer1);
                _createdViews.Add(type, controller);
            }
            
            _currentView = controller;
            _currentView.Show();
            
            return controller as TController;
        }

        public void HideCurrent()
        {
            _currentView?.Hide();
            _currentView = null;
        }

        public void CreateRoot() 
            => _root = _viewFabric.CreateRoot();

        public TController ShowPermanent<TView, TController>()
            where TView : View where TController : ViewController<TView>
        {
            var type = typeof(TView);
            
            if (!_createdViews.TryGetValue(type, out IViewController controller))
            {
                controller = _viewFabric.Create<TView, TController>(_root.Layer2);
                _createdViews.Add(type, controller);
            }
            
            controller.Show();
            _permanentViews.AddLast(controller);
            return controller as TController;
        }
        
        public void HidePermanent<TController>(TController controller) where TController : IViewController
        {
            var view = _permanentViews.Find(controller)?.Value;
            view.Hide();
            _permanentViews.Remove(view);
        }
    }
}