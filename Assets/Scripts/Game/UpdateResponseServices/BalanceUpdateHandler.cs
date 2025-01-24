using Game.MiniGames;
using Game.Perks;
using Infrastructure.Network;
using Infrastructure.Network.Request.Base.Player;
using Infrastructure.Network.Request.ValidationPayment;
using Infrastructure.Network.RequestHandler;
using Infrastructure.Network.Response;
using Player;

namespace Game.UpdateResponseServices
{
    public class BalanceUpdateHandler : ResponseHandler,
        IRequestHandler<GameData>,
        IRequestHandler<ValidationPaymentResponse>,
        IRequestHandler<PaymentUpgradePerkResult>,
        IRequestHandler<PaymentItemResult>
    {
        private readonly WalletService _walletService;
        private readonly PerksService _perksService;
        private readonly MiniGamesSystem _miniGamesSystem;

        public BalanceUpdateHandler(IServerRequestSender serverRequestSender,
            WalletService walletService, 
            PerksService perksService,
            MiniGamesSystem miniGamesSystem)
            : base(serverRequestSender)
        {
            _walletService = walletService;
            _perksService = perksService;
            _miniGamesSystem = miniGamesSystem;
        }

        public override void StartListening()
        {
            ServerRequestSender.AddHandler(new IRequestHandler<GameData>[] { this });
            ServerRequestSender.AddHandler(new IRequestHandler<ValidationPaymentResponse>[] { this });
            ServerRequestSender.AddHandler(new IRequestHandler<PaymentItemResult>[] { this });
            ServerRequestSender.AddHandler(new IRequestHandler<PaymentUpgradePerkResult>[] { this });
        }

        public override void StopListening()
        {
            ServerRequestSender.RemoveHandler(new IRequestHandler<GameData>[] { this });
            ServerRequestSender.RemoveHandler(new IRequestHandler<ValidationPaymentResponse>[] { this });
            ServerRequestSender.RemoveHandler(new IRequestHandler<PaymentItemResult>[] { this });
            ServerRequestSender.RemoveHandler(new IRequestHandler<PaymentUpgradePerkResult>[] { this });
        }

        public void HandleServerData(GameData response)
        {
            _walletService.UpdateValues(response.BalanceUpdate, response.Perks);
            _perksService.HandlePerks(response.Perks);
            _miniGamesSystem.HandleServerData(response);
        }

        public void HandleServerData(ValidationPaymentResponse response)
        {
            _walletService.UpdateValues(response.Balance, response.Perks);
            _perksService.HandlePerks(response.Perks);
        }

        public void HandleServerData(PaymentUpgradePerkResult response)
        {
            _walletService.UpdateValues(response.BalanceUpdate, response.PerkInfo);
            _perksService.HandlePerk(response.PerkInfo);
        }

        public void HandleServerData(PaymentItemResult response)
        {
            _walletService.UpdateValues(response.BalanceUpdate);
        }
    }
}