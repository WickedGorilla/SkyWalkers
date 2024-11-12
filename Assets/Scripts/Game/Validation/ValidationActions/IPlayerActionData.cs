namespace Game.Validation
{
    public interface IPlayerActionData
    {
        public ValidationAction.ValidationType ActionType { get; }
        
        public object GetObjectForJson();
    }
}