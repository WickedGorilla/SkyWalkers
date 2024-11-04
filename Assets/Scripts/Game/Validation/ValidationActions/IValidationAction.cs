namespace Game.Validation.ValidationActions
{
    public interface IValidationAction
    {
        public ValidationType ActionType { get; }
    }
    
    public enum ValidationType
    {
        TapCoins = 0,
        ActivatePlayPass = 1,
        ActivateBoost = 2,
        EndBoost = 3,
        TapCoinsWithBoost = 4
    }
}