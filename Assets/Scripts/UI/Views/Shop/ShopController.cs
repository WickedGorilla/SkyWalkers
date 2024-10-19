using Game.Perks;
using Infrastructure.Data.Game.Shop;
using Infrastructure.Network;
using Player;

namespace UI.Views
{
    public class ShopController : BaseShopController<ShopView>
    {
        public ShopController(ShopView view,
            IServerRequestSender serverRequestSender,
            PerksService perksService,
            ShopData data,
            WalletService walletService)
            : base(view, serverRequestSender, perksService, data, walletService)
        {
        }
    }
}