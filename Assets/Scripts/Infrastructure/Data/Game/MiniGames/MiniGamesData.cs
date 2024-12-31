using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames
{
    [CreateAssetMenu(fileName = "MiniGamesData", menuName = "ScriptableObjects/Minigames/MiniGamesData", order = 0)]
    public class MiniGamesData  : ScriptableObject
    { 
        [SerializeField] private PasswordMiniGameData _passwordMiniGame;
        [SerializeField] private Vector2Int _rangeTapsToStartMiniGame = new(90, 140);

        public PasswordMiniGameData PasswordMiniGame => _passwordMiniGame;
        public Vector2Int RangeTapsToStartMiniGame => _rangeTapsToStartMiniGame;
    }
}