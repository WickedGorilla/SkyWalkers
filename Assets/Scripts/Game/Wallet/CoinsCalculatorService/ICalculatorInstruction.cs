namespace Game.Wallet
{
    internal interface ICalculatorInstruction
    {
        int Calculate();
    }

    public class DefaultCalculatorInstruction : ICalculatorInstruction
    {
        public int Calculate() 
            => 1;
    }

    public class BoostMultiplierInstruction : ICalculatorInstruction
    {
        private readonly int _boostMultiplier;

        public BoostMultiplierInstruction(int boostMultiplier)
        {
            _boostMultiplier = boostMultiplier;
        }
        
        public int Calculate()
        {
            return 1 * _boostMultiplier;
        }
    }
}