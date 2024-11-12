namespace Game.Validation
{
    public struct ActivateBoostActionData : IPlayerActionData
    {
        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.ActivateBoost;
        public object GetObjectForJson() 
            => new { };
    }

    public struct BoostEndActionData : IPlayerActionData
    {
        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.EndBoost;
        public object GetObjectForJson() 
            => new { };
    }
}