using DG.Tweening;
using Game.Effects;
using Game.Player;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentObjects : MonoBehaviour
    {
        [SerializeField] private BuildingPositionOnTheScreen _sitBuilding;
        [SerializeField] private BuildingPositionOnTheScreen _buildingRoot;
        [SerializeField] private ParticleSystem _coinsParticle;
        [SerializeField] private SpriteChanger _sun;
        
        [SerializeField] private Transform _dynamicLayer1;
        [SerializeField] private Transform _dynamicLayer2;
        [SerializeField] private Transform _dynamicLayer3;
        [SerializeField] private Transform _lowerLevel;
        
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private PlayerAnimation _player;
        
        [Header("MiniGames parameters")]
        [SerializeField] private EnvironmentAnimation _quadrocopter;
        [SerializeField] private EnvironmentAnimation _securityGuardMan;
        [SerializeField] private EnvironmentAnimation _platformConstruction;
        [SerializeField] private SpriteRenderer _stopLine;
        [SerializeField] private ParticleSystem _rainParticle;

        
        private Vector3 _defaultSitGroupPosition;
        private Vector3 _defaultClimbGroupPosition;
        private Vector3 _defaultSunPosition;
        
        public PlayerAnimation Player => _player;
        public Transform BuildingRoot => _buildingRoot.transform;
        public Transform LowerLevel => _lowerLevel;
        public Transform DynamicLayer1 => _dynamicLayer1;
        public Transform DynamicLayer2 => _dynamicLayer2;
        public Transform DynamicLayer3 => _dynamicLayer3;
        public ParticleSystem CoinsParticle => _coinsParticle;

        public bool Animated { get; private set; }

        public EnvironmentAnimation Quadrocopter => _quadrocopter;
        public EnvironmentAnimation SecurityGuardMan => _securityGuardMan;
        public SpriteRenderer StopLine => _stopLine;
        public ParticleSystem RainParticle => _rainParticle;
        public EnvironmentAnimation PlatformConstruction => _platformConstruction;

        private void Start()
        {
            _defaultSitGroupPosition = _sitBuilding.transform.position;
            _defaultClimbGroupPosition = _buildingRoot.transform.position;
            _defaultSunPosition = _sun.transform.localPosition;
            
            _buildingRoot.gameObject.SetActive(false);
            _sitBuilding.gameObject.SetActive(true);

            _buildingRoot.transform.position = new Vector3(_defaultClimbGroupPosition.x + 10f, _defaultClimbGroupPosition.y);
            _sun.transform.localPosition = new Vector3(_defaultSunPosition.x, _defaultSunPosition.y + 10f);
        }

        public void ShowBuildingGroup()
        {
            _sitBuilding.enabled = false;
            Animated = true;
            
            _sitBuilding.transform.DOMoveX(_defaultSitGroupPosition.x - 10f, _animationDuration)
                .OnComplete(() => _sitBuilding.gameObject.SetActive(false));
            
            _buildingRoot.gameObject.SetActive(true);
            _buildingRoot.transform.DOMoveX(_defaultClimbGroupPosition.x, _animationDuration * 2)
                .OnComplete(() =>
                {
                    _buildingRoot.enabled = true;
                    Animated = false;
                });
        }

        public void ShowSitGroup()
        {
            Animated = true;
            _sitBuilding.enabled = true;
            _sitBuilding.gameObject.SetActive(true);
            _sitBuilding.transform.DOMoveX(_defaultSitGroupPosition.x, _animationDuration)
                .OnComplete(() => _sitBuilding.enabled = true);
            
            _buildingRoot.enabled = false;
            _buildingRoot.transform.DOMoveX(_defaultClimbGroupPosition.x + 10f, _animationDuration)
                .OnComplete(() =>
                {
                    _buildingRoot.gameObject.SetActive(false);
                    Animated = false;
                });
        }

        public void ShowSun()
        {
            _sun.gameObject.SetActive(true);
            _sun.transform.DOLocalMoveY(_defaultSunPosition.y, _animationDuration);
        }
        
        public void HideSun()
        {
            _sun.transform.DOLocalMoveY(_defaultSunPosition.y + 10f, _animationDuration)
                .OnComplete(() => _sun.gameObject.SetActive(false));
        }
    }
}