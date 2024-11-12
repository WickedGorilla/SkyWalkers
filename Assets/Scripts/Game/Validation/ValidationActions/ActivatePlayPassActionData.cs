namespace Game.Validation
{
    public struct ActivatePlayPassActionData : IPlayerActionData
    {
        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.ActivatePlayPass;
        public object GetObjectForJson() 
            => new { };
    }
}