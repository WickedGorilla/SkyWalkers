namespace Game.Validation.ValidationActions
{
    public struct ActivateBoostAction : IValidationAction
    {
        public ValidationType ActionType => ValidationType.ActivateBoost;
    }

    public struct BoostEndAction : IValidationAction
    {
        public ValidationType ActionType => ValidationType.EndBoost;
    }
}