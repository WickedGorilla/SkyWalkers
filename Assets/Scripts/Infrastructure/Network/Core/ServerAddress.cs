namespace Infrastructure.Network
{
    public class ServerAddress
    {
        public const string Login = "api/Auth/login";
        public const string PaymentItem = "api/Payment/BuyItem";
        public const string PaymentPerk = "api/Payment/BuyUpgrade";
        public const string PaymentValidation = "api/Payment/ValidatePayment";
        public static string TapCoinsValidation = "api/CoinsValidation/Validate";
    }
}