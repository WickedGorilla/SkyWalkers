using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames.RainMiniGame
{
    [CreateAssetMenu(fileName = "RainMiniGameData", menuName = "ScriptableObjects/Minigames/RainMiniGameData", order = 0)]
    public class RainMiniGameData : ScriptableObject
    {
        [SerializeField] private int _timeForMiniGame = 15;
        [SerializeField] private float _forceByClick = 1f;
        [SerializeField] private float _forceDecayRate = 0.5f;
        [SerializeField] private float _gravity = 1f;
        [SerializeField] private float _minY = -5f;
        [SerializeField] private float _maxY = 5f;      
        [SerializeField] private float _failPlayerPosition = -5f;      
        
        public int TimeForMiniGame => _timeForMiniGame;
        public float ForceByClick => _forceByClick;
        public float Gravity => _gravity;

        public float MinY => _minY;
        public float MaxY => _maxY;
        public float ForceDecayRate => _forceDecayRate;
        public float FailPlayerPosition => _failPlayerPosition;
    }
}