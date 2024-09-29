namespace Game.Wallet
{
    public class CoinsCalculatorService
    {
        private readonly ICalculatorInstruction _defaultInstruction;
        
        private ICalculatorInstruction _calculatorInstruction;

        public CoinsCalculatorService()
        {
            _defaultInstruction = new DefaultCalculatorInstruction();
            _calculatorInstruction = _defaultInstruction;
        }
        
        public int CalculateCoinsByTap()
            => _calculatorInstruction.Calculate();

        public void UpdateInstruction(int boostMultiplier)
            => _calculatorInstruction = new BoostMultiplierInstruction(boostMultiplier);
        
        public void ResetInstruction()
            => _calculatorInstruction = _defaultInstruction;
    }
}