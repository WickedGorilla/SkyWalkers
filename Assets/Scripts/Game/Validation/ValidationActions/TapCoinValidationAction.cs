namespace Game.Validation.ValidationActions
{
    public struct TapCoinValidationAction : IValidationAction
    {
        public int CoinsCount;
        public int EnergyCount;

        public TapCoinValidationAction(int coinsCount, int energyCount)
        {
            CoinsCount = coinsCount;
            EnergyCount = energyCount;
        }
        
        public ValidationType ActionType => ValidationType.TapCoins;
    }
}