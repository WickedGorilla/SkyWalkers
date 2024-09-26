namespace Player
{
    public class WalletService
    {
        public readonly BalanceValue Coins = new();
        public readonly BalanceValue Energy = new();
        public readonly BalanceValue Boosts = new();

        public void Initialize(int coins, int energy, int boosts)
        {
            Coins.Initialize(coins);
            Energy.Initialize(energy);
            Boosts.Initialize(boosts);
        }
    }
}