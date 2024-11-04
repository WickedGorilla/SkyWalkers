namespace Game.Validation.ValidationActions
{
    public interface IValidationAction
    {
        public ValidationType ActionType { get; }
    }
    
    public enum ValidationType
    {
        TapCoins,
        ActivatePlayPass,
        ActivateBoost
    }
}