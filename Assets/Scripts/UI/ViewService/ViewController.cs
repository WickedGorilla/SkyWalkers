using System;
using System.Collections.Generic;

namespace UI.Core
{
    public abstract class ViewController<TView>  : IViewController where TView : View
    {
        private readonly LinkedList<Action> _onCloseActions; 
        
        protected TView View { get; }
        
        protected ViewController(TView view)
        {
            View = view;
            _onCloseActions = new LinkedList<Action>();
        }
        
        public void Show()
        {
            View.gameObject.SetActive(true);
            View.OnShow();
            OnShow();
        }

        public void Hide()
        {
            OnHide();
            View.OnHide();
            View.gameObject.SetActive(false);
            
            if (_onCloseActions.Count == 0)
                return;
            
            foreach (Action onClose in _onCloseActions)
                onClose();
            
            _onCloseActions.Clear();
        }

        public void AddOnCloseEvent(Action onClose) 
            => _onCloseActions.AddLast(onClose);
        
        protected virtual void OnHide()
        {
            
        }

        protected virtual void OnShow()
        {
            
        }
    }
}