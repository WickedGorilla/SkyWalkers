using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames.ConstructionMiniGame
{
    [CreateAssetMenu(fileName = "ConstructionMiniGameData",
        menuName = "ScriptableObjects/Minigames/ConstructionMiniGameData", order = 0)]
    public class ConstructionMiniGameData : ScriptableObject
    {
        [SerializeField] private int _timeForMiniGame = 15;
        [SerializeField] private int _reservePowerControl = 15;
        [SerializeField] private Vector2 _powerRangeForLeft = new(2, 5);
        [SerializeField] private Vector2 _powerRangeForRight = new(1, 2);
        [SerializeField] private float _forceDecayRate = 0.5f;

        public int TimeForMiniGame => _timeForMiniGame;

        public int ReservePowerControl => _reservePowerControl;

        public Vector2 PowerRangeForLeft => _powerRangeForLeft;
        public Vector2 PowerRangeForRight => _powerRangeForRight;
        public float ForceDecayRate => _forceDecayRate;
    }
}