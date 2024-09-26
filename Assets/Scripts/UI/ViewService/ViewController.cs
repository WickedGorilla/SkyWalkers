using UnityEngine;

namespace UI.ViewService
{
    public abstract class ViewController<TView>  : IViewController where TView : View
    {
        protected TView View { get; }

        protected ViewController(TView view)
        {
            View = view;
        }
        
        public void Show()
        {
            View.OnShow();
            OnShow();
        }

        public void Hide()
        {
            OnHide();
            View.OnHide();
            Object.Destroy(View.gameObject);
        }

        protected virtual void OnHide()
        {
            
        }

        protected virtual void OnShow()
        {
            
        }
    }
}