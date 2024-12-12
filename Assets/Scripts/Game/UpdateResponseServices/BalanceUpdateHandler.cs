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

        public BalanceUpdateHandler(IServerRequestSender serverRequestSender,
            WalletService walletService, PerksService perksService)
            : base(serverRequestSender)
        {
            _walletService = walletService;
            _perksService = perksService;
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

        public void Handle(GameData response)
        {
            _walletService.UpdateValues(response.BalanceUpdate, response.Perks);
            _perksService.HandlePerks(response.Perks);
        }

        public void Handle(ValidationPaymentResponse response)
        {
            _walletService.UpdateValues(response.Balance, response.Perks);
            _perksService.HandlePerks(response.Perks);
        }

        public void Handle(PaymentUpgradePerkResult response)
        {
            _walletService.UpdateValues(response.BalanceUpdate, response.PerkInfo);
            _perksService.HandlePerk(response.PerkInfo);
        }

        public void Handle(PaymentItemResult response)
        {
            _walletService.UpdateValues(response.BalanceUpdate);
        }
    }
}