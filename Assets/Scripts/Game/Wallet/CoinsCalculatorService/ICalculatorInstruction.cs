using Game.Perks;

namespace Game.Wallet
{
    internal interface ICalculatorInstruction
    {
        int Calculate();
    }

    public class DefaultCalculatorInstruction : ICalculatorInstruction
    {
        private readonly PerksService _perksService;

        public DefaultCalculatorInstruction(PerksService perksService) 
            => _perksService = perksService;

        public int Calculate() 
            => _perksService.MultiTap.CurrentValue;
    }

    public class BoostMultiplierInstruction : ICalculatorInstruction
    {
        private readonly PerksService _perksService;
        private readonly int _boostMultiplier;

        public BoostMultiplierInstruction(PerksService perksService, int boostMultiplier)
        {
            _perksService = perksService;
            _boostMultiplier = boostMultiplier;
        }
        
        public int Calculate() 
            => _perksService.MultiTap.CurrentValue * _boostMultiplier;
    }
}