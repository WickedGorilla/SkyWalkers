using Infrastructure.Data.Game.MiniGames.PasswordMiniGame;
using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames
{
    [CreateAssetMenu(fileName = "PasswordMiniGameData", menuName = "ScriptableObjects/Minigames/PasswordMiniGameData", order = 0)]
    public class PasswordMiniGameData : ScriptableObject
    {
        [SerializeField] private PasswordVariable[] _passwords;
        [SerializeField] private int _countRounds = 3;
        [SerializeField] private int _countMistakes = 3;
        [SerializeField] private int _timeForMiniGame = 15;

        public int CountRounds => _countRounds;
        public int CountMistakes => _countMistakes;
        public int TimeForMiniGame => _timeForMiniGame;

        public PasswordVariable GetRandomPassword()
        {
            int randomIndex = Random.Range(0, _passwords.Length);
            return _passwords[randomIndex];
        }
    }
}