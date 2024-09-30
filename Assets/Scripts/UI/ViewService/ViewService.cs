using System.Collections.Generic;

namespace UI.Core
{
    public class ViewService
    {
        private readonly ViewFabric _viewFabric;
        private readonly LinkedList<IViewController> _permanentViews;
        
        private IViewController _currentView;
        private UIRoot _root;

        public ViewService(ViewFabric viewFabric)
        {
            _viewFabric = viewFabric;
            _permanentViews = new LinkedList<IViewController>();
        }

        public TController Show<TView, TController>()
            where TView : View where TController : ViewController<TView>
        {
            _currentView?.Hide();
            TController controller = _viewFabric.Create<TView, TController>(_root.Layer1);
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

        public TController ShowPermanent<TView, TController>()
            where TView : View where TController : ViewController<TView>
        {
            TController controller = _viewFabric.Create<TView, TController>(_root.Layer2);
            controller.Show();
            _permanentViews.AddLast(controller);
            return controller;
        }
        
        public void HidePermanent<TController>(TController controller) where TController : IViewController
        {
            var view = _permanentViews.Find(controller)?.Value;
            view.Hide();
            _permanentViews.Remove(view);
        }
    }
}