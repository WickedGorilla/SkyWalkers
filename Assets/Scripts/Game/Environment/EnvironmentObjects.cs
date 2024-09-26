using DG.Tweening;
using Game.Player;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentObjects : MonoBehaviour
    {
        [SerializeField] private BuildingPositionOnTheScreen _sitBuilding;
        [SerializeField] private Transform _sitCharacterGroup;
        
        [SerializeField] private Transform _climbCharacterGroup;
        [SerializeField] private BuildingPositionOnTheScreen _buildingRoot;
        [SerializeField] private Transform _dynamicLayer1;
        [SerializeField] private Transform _dynamicLayer2;
        [SerializeField] private Transform _lowerLevel;

        [SerializeField] private float _animationDuration = 1f;

        [SerializeField] private PlayerAnimation _player;
        
        private Vector3 _defaultSitGroupPosition;
        private Vector3 _defaultClimbGroupPosition;

        public PlayerAnimation Player => _player;
        public Transform BuildingRoot => _buildingRoot.transform;
        public Transform LowerLevel => _lowerLevel;
        public Transform DynamicLayer1 => _dynamicLayer1;
        public Transform DynamicLayer2 => _dynamicLayer2;

        private void Start()
        {
            _defaultSitGroupPosition = _sitCharacterGroup.position;
            _defaultClimbGroupPosition = _climbCharacterGroup.position;
            
            _climbCharacterGroup.gameObject.SetActive(false);
            _sitCharacterGroup.gameObject.SetActive(true);
        }

        public void ShowBuildingGroup()
        {
            _sitBuilding.enabled = false;
            
            _sitCharacterGroup.DOMoveX(_defaultSitGroupPosition.x - 10f, _animationDuration)
                .OnComplete(() => _sitCharacterGroup.gameObject.SetActive(false));
            
            _climbCharacterGroup.gameObject.SetActive(true);
            _climbCharacterGroup.DOMoveX(_defaultClimbGroupPosition.x, _animationDuration)
                .OnComplete(() => _buildingRoot.enabled = true);
        }

        public void ShowSitGroup()
        {
            _sitCharacterGroup.gameObject.SetActive(true);
            _sitCharacterGroup.DOMoveX(_defaultSitGroupPosition.x, _animationDuration)
                .OnComplete(() => _sitBuilding.enabled = true);
            
            _buildingRoot.enabled = false;
            _climbCharacterGroup.DOMoveX(_defaultClimbGroupPosition.x + 10f, _animationDuration)
                .OnComplete(() => _climbCharacterGroup.gameObject.SetActive(false));
        } 
    }
}