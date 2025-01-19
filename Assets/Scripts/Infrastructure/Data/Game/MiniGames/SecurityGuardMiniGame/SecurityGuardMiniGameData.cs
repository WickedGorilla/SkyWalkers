using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames.SecurityGuardMiniGame
{
    [CreateAssetMenu(fileName = "SecurityGuardMiniGameData", menuName = "ScriptableObjects/Minigames/SecurityGuardMiniGameData", order = 0)]
    public class SecurityGuardMiniGameData : ScriptableObject
    {
        [SerializeField] private int _earnForComplete = 7;
        [SerializeField] private int _timeForMiniGame = 15;
        [SerializeField] private int _countMistakeForFail = 3;

        public int EarnForComplete => _earnForComplete;
        public int TimeForMiniGame => _timeForMiniGame;
        public int CountMistakeForFail => _countMistakeForFail;
    }
}