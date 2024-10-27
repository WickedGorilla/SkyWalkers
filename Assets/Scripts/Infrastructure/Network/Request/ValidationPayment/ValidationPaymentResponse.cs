using Infrastructure.Network.Response.Player;

namespace Infrastructure.Network.Request.ValidationPayment
{
    public class ValidationPaymentResponse
    {
        public bool IsUpdated;
        public BalanceUpdate BalanceUpdate;
        public PerksInfo PerksInfo;
    }
}