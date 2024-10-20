using Game.Perks;

namespace Game.Wallet
{
    public class CoinsCalculatorService
    {
        private readonly PerksService _perksService;
        private readonly ICalculatorInstruction _defaultInstruction;
        
        private ICalculatorInstruction _calculatorInstruction;

        public CoinsCalculatorService(PerksService perksService)
        {
            _perksService = perksService;
            _defaultInstruction = new DefaultCalculatorInstruction(_perksService);
            _calculatorInstruction = _defaultInstruction;
        }
        
        public int CalculateCoinsByTap()
            => _calculatorInstruction.Calculate();

        public void UpdateInstruction(int boostMultiplier)
            => _calculatorInstruction = new BoostMultiplierInstruction(_perksService, boostMultiplier);
        
        public void ResetInstruction()
            => _calculatorInstruction = _defaultInstruction;
    }
}