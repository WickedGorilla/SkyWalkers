using UnityEngine;

namespace Game.BuildingSystem
{
    public class AnimatorBackground
    {
        private IDirection _currentDirection;
        private IDirection _nextDirection;

        public void Initialize(Transform transform)
        {
            _currentDirection = new LeftDirection(transform.position.x -0.1f, 0.1f);
            _nextDirection = new RightDirection(transform.position.x + 0.1f, 0.1f);
        }
        
        public void Animate(Transform transform)
        {
            if (_currentDirection.Move(transform)) 
                return;
            
            (_currentDirection, _nextDirection) = (_nextDirection, _currentDirection);
            _currentDirection.Move(transform);
        }

        interface IDirection
        {
            bool Move(Transform transform);
        }

        class LeftDirection : IDirection
        {
            private readonly float _minPosition;
            private readonly float _speed;

            public LeftDirection(float minPosition, float speed)
            {
                _minPosition = minPosition;
                _speed = speed;
            }
            
            public bool Move(Transform transform)
            {
                if (transform.position.x <= _minPosition)
                    return false;
                
                var pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * _speed, pos.y);
                
                return true;
            }
        }
        
        class RightDirection : IDirection
        {
            private readonly float _maxPosition;
            private readonly float _speed;

            public RightDirection(float maxPosition, float speed)
            {
                _maxPosition = maxPosition;
                _speed = speed;
            }
            
            public bool Move(Transform transform)
            {
                if (transform.position.x >= _maxPosition)
                    return false;
                
                var pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * _speed, pos.y);
                
                return true;
            }
        }
    }
}