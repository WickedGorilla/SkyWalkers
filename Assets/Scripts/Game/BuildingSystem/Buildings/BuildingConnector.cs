using UnityEngine;

namespace Game.BuildingSystem
{
    public class BuildingConnector : MonoBehaviour
    {
        [SerializeField] private BorderZone _border;

        public BorderZone Border => _border;
        public Vector2 Position => transform.position;

        public void ConnectDownTo(Vector2 position) 
            => transform.position = position + (new Vector2(0f, _border.Size.y / 2) - _border.Center) * transform.lossyScale;

        public void ConnectUpperTo(Vector2 position) 
            => transform.position = position - (new Vector2(0f, _border.Size.y / 2) - _border.Center) * transform.lossyScale;

        public void MoveY(float adderValue) 
            => transform.position = new Vector2(Position.x, Position.y - adderValue);
    }
}