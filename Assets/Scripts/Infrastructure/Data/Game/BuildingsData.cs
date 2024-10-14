using Game.BuildingSystem;
using UnityEngine;

namespace Infrastructure.Data.Game
{
    [CreateAssetMenu(fileName = "BuildingsData", menuName = "ScriptableObjects/BuildingsData", order = 0)]
    public class BuildingsData : ScriptableObject
    {
        [SerializeField] private float _speedBuildings;
        [SerializeField] private float _minimumPosition; 
        [SerializeField] private float _startSpawnPosition;
        [SerializeField] private  float _minLowerLevelPosition = -5f;
        [SerializeField] private  float _moveLowerLevelSpeed = 0.5f;
        
        [Space]
        [SerializeField] private BuildingConnector[] _buildingsPrefabs;
        
        public BuildingConnector[] BuildingsPrefabs => _buildingsPrefabs;
        public float SpeedBuildings => _speedBuildings;
        public float MinimumPosition => _minimumPosition;
        public float StartSpawnPosition => _startSpawnPosition;
        
        public float MinLowerLevelPosition => _minLowerLevelPosition;
        public float MoveLowerLevelSpeed => _moveLowerLevelSpeed;
    }
}