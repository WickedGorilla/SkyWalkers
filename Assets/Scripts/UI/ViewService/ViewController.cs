namespace UI.Core
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
            View.gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            OnHide();
            View.OnHide();
            View.gameObject.SetActive(false);
        }

        protected virtual void OnHide()
        {
            
        }

        protected virtual void OnShow()
        {
            
        }
    }
}