namespace Game.Validation
{
    public struct CoinPlayerActionData : ICoinPlayerActionData
    {
        public int CoinsCount;
        public int EnergyCount;

        public CoinPlayerActionData(int coinsCount, int energyCount)
        {
            CoinsCount = coinsCount;
            EnergyCount = energyCount;
        }

        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.TapCoins;

        public object GetObjectForJson() 
            => new { CoinsCount, EnergyCount };
    }

    public struct CoinWithBoostActionData : ICoinPlayerActionData
    {
        public int CoinsCount;

        public CoinWithBoostActionData(int coinsCount)
        {
            CoinsCount = coinsCount;
        }

        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.TapCoinsWithBoost;
        
        public object GetObjectForJson() 
            => new { CoinsCount };
    }

    public interface ICoinPlayerActionData : IPlayerActionData
    {
    }
}