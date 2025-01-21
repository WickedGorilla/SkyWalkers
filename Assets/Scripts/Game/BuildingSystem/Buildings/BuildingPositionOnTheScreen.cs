using UnityEngine;

public class BuildingPositionOnTheScreen : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private ScreenPosition _screenAnchoring;

    private Vector2 _screenPosition;
    private Vector2Int _previousScreenSize;
    private int _frameIndex;
    private float _zPosition;

    private void Awake()
    {
        UpdateScreenSize();
        _screenPosition = GetScreenPosition();
        
        _zPosition = transform.position.z;
        DoPositionOnScreen();
    }
    
    private void Update()
    {
        _frameIndex++;
        
        if (_frameIndex % 5 != 0)
            return;
        
        if (_previousScreenSize.x == Screen.width 
            && _previousScreenSize.y == Screen.height)
            return;
        
        UpdateScreenSize();
        DoPositionOnScreen();
    }

    private void DoPositionOnScreen()
    {
        Vector3 screenPosition = new Vector3(_screenPosition.x, _screenPosition.y, _camera.nearClipPlane);
        Vector3 worldPosition = _camera.ViewportToWorldPoint(screenPosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, _zPosition);
    }

    private void UpdateScreenSize() 
        => _previousScreenSize = new Vector2Int(Screen.width, Screen.height);

    private Vector2 GetScreenPosition()
    {
        return _screenAnchoring switch
        {
            ScreenPosition.UpperLeft => new Vector2(0f, 1f),
            ScreenPosition.MiddleRight => new Vector2(1f, 0.5f),
            _ => Vector2.zero
        };
    }
    
    private enum ScreenPosition
    {
        UpperLeft,
        MiddleRight
    }
}