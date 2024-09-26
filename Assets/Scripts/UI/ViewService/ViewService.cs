using UnityEngine;

namespace UI.ViewService
{
    public class ViewService
    {
        private readonly ViewFabric _viewFabric;
        
        private IViewController _currentView;
        private Canvas _root;

        public ViewService(ViewFabric viewFabric)
        {
            _viewFabric = viewFabric;
        }

        public TController Show<TView, TController>()
            where TView : View where TController : ViewController<TView>
        {
            _currentView?.Hide();
            TController controller = _viewFabric.Create<TView, TController>(_root.transform);
            _currentView = controller;
            _currentView.Show();
            return controller;
        }

        public void HideCurrent()
        {
            _currentView?.Hide();
            _currentView = null;
        }

        public void CreateRoot() 
            => _root = _viewFabric.CreateRoot();
    }
}