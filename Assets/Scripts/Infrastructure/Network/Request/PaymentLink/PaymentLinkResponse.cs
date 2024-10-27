using Infrastructure.Network.Response.Player;

namespace Infrastructure.Network.Response
{
    public class PaymentItemResult
    {
        public BalanceUpdate BalanceUpdate;
        public string PaymentUrl;

        public PaymentItemResult(BalanceUpdate balanceUpdate, string paymentUrl = "")
        {
            BalanceUpdate = balanceUpdate;
            PaymentUrl = paymentUrl;
        }
    }

    public class PaymentUpgradePerkResult
    {
        public PerkInfo PerkInfo;
        public BalanceUpdate BalanceUpdate;
        public string PaymentUrl;
    
        public PaymentUpgradePerkResult(PerkInfo perkInfo, BalanceUpdate balanceUpdate, string paymentUrl = "")
        {
            PerkInfo = perkInfo;
            BalanceUpdate = balanceUpdate;
            PaymentUrl = paymentUrl;
        }
    }
}