using Infrastructure.Data.Game.MiniGames.ConstructionMiniGame;
using Infrastructure.Data.Game.MiniGames.RainMiniGame;
using Infrastructure.Data.Game.MiniGames.SecurityGuardMiniGame;
using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames
{
    [CreateAssetMenu(fileName = "MiniGamesData", menuName = "ScriptableObjects/Minigames/MiniGamesData", order = 0)]
    public class MiniGamesData  : ScriptableObject
    { 
        [SerializeField] private float _delayToStartMiniGame = 1.5f;
        [SerializeField] private Vector2Int _rangeTapsToStartMiniGame = new(90, 140);
        
        [Header("Mini games data")]
        [SerializeField] private PasswordMiniGameData _passwordMiniGame;
        [SerializeField] private SecurityGuardMiniGameData _securityGuardMiniGame;
        [SerializeField] private RainMiniGameData _rainMiniGameData;
        [SerializeField] private ConstructionMiniGameData _constructionMiniGameData;

        public Vector2Int RangeTapsToStartMiniGame => _rangeTapsToStartMiniGame;
        public float DelayToStartMiniGame => _delayToStartMiniGame;
        
        public PasswordMiniGameData PasswordMiniGame => _passwordMiniGame;
        public SecurityGuardMiniGameData SecurityGuardMiniGame => _securityGuardMiniGame;
        public RainMiniGameData RainMiniGameData => _rainMiniGameData;
        public ConstructionMiniGameData ConstructionMiniGameData => _constructionMiniGameData;
    }
}