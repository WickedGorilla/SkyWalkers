using Game.MiniGames;

namespace Game.Validation
{
    public struct EnterMiniGameActionData : IPlayerActionData
    {
        private readonly MiniGameType _miniGameType;
        private readonly int _tappedCoins;

        public EnterMiniGameActionData(MiniGameType miniGameType, int tappedCoins)
        {
            _miniGameType = miniGameType;
            _tappedCoins = tappedCoins;
        }

        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.EnterMiniGame;

        public object GetObjectForJson()
            => new
            {
                IndexMiniGame = (int)_miniGameType,
                TappedCoins = _tappedCoins
            };
    }

    public struct EndMiniGameActionData : IPlayerActionData
    {
        private readonly bool _isCompete;

        public EndMiniGameActionData(bool isCompete) 
            => _isCompete = isCompete;

        public ValidationAction.ValidationType ActionType => ValidationAction.ValidationType.EndMiniGame;

        public object GetObjectForJson()
            => new { IsComplete = _isCompete };
    }
}