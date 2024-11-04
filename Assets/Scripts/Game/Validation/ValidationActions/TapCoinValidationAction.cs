namespace Game.Validation.ValidationActions
{
    public struct CoinValidationAction : ICoinValidationAction
    {
        public int CoinsCount;
        public int EnergyCount;

        public CoinValidationAction(int coinsCount, int energyCount)
        {
            CoinsCount = coinsCount;
            EnergyCount = energyCount;
        }
        
        public ValidationType ActionType => ValidationType.TapCoins;
    }

    public struct CoinWithBoostAction : ICoinValidationAction
    {
        public int CoinsCount;
        
        public CoinWithBoostAction(int coinsCount)
        {
            CoinsCount = coinsCount;
        }
        
        public ValidationType ActionType => ValidationType.TapCoinsWithBoost;
    }

    public interface ICoinValidationAction : IValidationAction
    {
        
    }
}