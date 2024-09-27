using DG.Tweening;
using Game.Player;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentObjects : MonoBehaviour
    {
        [SerializeField] private BuildingPositionOnTheScreen _sitBuilding;
        [SerializeField] private BuildingPositionOnTheScreen _buildingRoot;
        
        [SerializeField] private Transform _dynamicLayer1;
        [SerializeField] private Transform _dynamicLayer2;
        [SerializeField] private Transform _dynamicLayer3;
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
        public Transform DynamicLayer3 => _dynamicLayer3;

        private void Start()
        {
            _defaultSitGroupPosition = _sitBuilding.transform.position;
            _defaultClimbGroupPosition = _buildingRoot.transform.position;
            
            _buildingRoot.gameObject.SetActive(false);
            _sitBuilding.gameObject.SetActive(true);

            Vector3 pos = _buildingRoot.transform.position;
            _buildingRoot.transform.position = new Vector3(_defaultClimbGroupPosition.x + 10f, pos.x, pos.z);
        }

        public void ShowBuildingGroup()
        {
            _sitBuilding.enabled = false;
            
            _sitBuilding.transform.DOMoveX(_defaultSitGroupPosition.x - 10f, _animationDuration)
                .OnComplete(() => _sitBuilding.gameObject.SetActive(false));
            
            _buildingRoot.gameObject.SetActive(true);
            _buildingRoot.transform.DOMoveX(_defaultClimbGroupPosition.x, _animationDuration * 2)
                .OnComplete(() => _buildingRoot.enabled = true);
        }

        public void ShowSitGroup()
        {
            _sitBuilding.enabled = true;
            _sitBuilding.gameObject.SetActive(true);
            _sitBuilding.transform.DOMoveX(_defaultSitGroupPosition.x, _animationDuration)
                .OnComplete(() => _sitBuilding.enabled = true);
            
            _buildingRoot.enabled = false;
            _buildingRoot.transform.DOMoveX(_defaultClimbGroupPosition.x + 10f, _animationDuration)
                .OnComplete(() => _buildingRoot.gameObject.SetActive(false));
        } 
    }
}