using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Data.Effects
{
    [CreateAssetMenu(fileName = "CoinsSpawnerData", menuName = "ScriptableObjects/CoinsSpawnerData", order = 0)]
    public class CoinsSpawnerData : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private Image _coinPrefab;
        [SerializeField] private TMP_Text _textPrefab;

        [Header("Config")] 
        [SerializeField] private float _moveDistance = 200f;
        [SerializeField] private float _coinAnimationDuration = 2f;
        [SerializeField] private float _textAnimationDuration = 1.5f;
        [SerializeField] private int _numberOfCoins = 5;
        [SerializeField] private Vector2 _textOffset = new(100f, 20f);
        [SerializeField] float _minRadius = 0f;
        [SerializeField] float _maxRadius = 100f;
        [SerializeField] float _startAngle = 45f;
        [SerializeField] float _endAngle = 135f;

        public Image CoinPrefab => _coinPrefab;
        public TMP_Text TextPrefab => _textPrefab;
        public float MoveDistance => _moveDistance;
        public float CoinAnimationDuration => _coinAnimationDuration;
        public float TextAnimationDuration => _textAnimationDuration;
        public Vector2 TextOffset => _textOffset;
        public float MinRadius => _minRadius;
        public float MaxRadius => _maxRadius;
        public float StartAngle => _startAngle;
        public float EndAngle => _endAngle;
    }
}