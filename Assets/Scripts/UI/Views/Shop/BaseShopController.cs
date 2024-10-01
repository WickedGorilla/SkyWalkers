using UI.Core;

namespace UI.Views
{
    public class ShopController : BaseShopController<ShopView>
    {
        protected ShopController(ShopView view) : base(view)
        {
        }
    }

    public abstract class BaseShopController<TShopView> : ViewController<TShopView> where TShopView : ShopView
    {
        protected BaseShopController(TShopView view) : base(view)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
            
        }
    }
}