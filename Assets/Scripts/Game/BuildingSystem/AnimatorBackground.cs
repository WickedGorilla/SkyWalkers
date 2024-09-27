using UnityEngine;

namespace Game.BuildingSystem
{
    public class AnimatorBackground
    {
        private IDirection _currentDirection;
        private IDirection _nextDirection;

        public void Initialize(Transform transform, bool isDownFirst = false, float speed = 0.06f)
        {
            if (isDownFirst)
            {
                _currentDirection = new DownDirection(transform.position.y -0.2f, speed);
                _nextDirection = new UpDirection(transform.position.y + 0.2f, speed);
            }
            else
            {
                _currentDirection = new UpDirection(transform.position.y + 0.2f, speed);
                _nextDirection =  new DownDirection(transform.position.y -0.2f, speed);
            }
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

        class DownDirection : IDirection
        {
            private readonly float _minPosition;
            private readonly float _speed;

            public DownDirection(float minPosition, float speed)
            {
                _minPosition = minPosition;
                _speed = speed;
            }
            
            public bool Move(Transform transform)
            {
                if (transform.position.y <= _minPosition)
                    return false;
                
                var pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y - Time.deltaTime * _speed);
                
                return true;
            }
        }
        
        class UpDirection : IDirection
        {
            private readonly float _maxPosition;
            private readonly float _speed;

            public UpDirection(float maxPosition, float speed)
            {
                _maxPosition = maxPosition;
                _speed = speed;
            }
            
            public bool Move(Transform transform)
            {
                if (transform.position.y >= _maxPosition)
                    return false;
                
                var pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * _speed);
                
                return true;
            }
        }
    }
}