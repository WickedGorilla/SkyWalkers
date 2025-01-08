using Game.Minigames;

namespace Game.Validation
{
    public struct EnterMiniGameActionData : IPlayerActionData
    {
        private readonly MiniGameType _miniGameType;

        public EnterMiniGameActionData(MiniGameType miniGameType) 
            => _miniGameType = miniGameType;

        public ValidationAction.ValidationType ActionType  => ValidationAction.ValidationType.EnterMiniGame;
        
        public object GetObjectForJson()
        {
            int indexMiniGame = (int)_miniGameType;
            return new { indexMiniGame };
        }
    }
    
    public struct EndMiniGameActionData : IPlayerActionData
    {
        private readonly bool _isCompete;

        public EndMiniGameActionData(bool isCompete) 
            => _isCompete = isCompete;

        public ValidationAction.ValidationType ActionType  => ValidationAction.ValidationType.EndMiniGame;
        
        public object GetObjectForJson() 
            => new { _isCompete };
    }
}