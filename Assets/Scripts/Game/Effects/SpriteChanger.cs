namespace Game.Effects
{
    using UnityEngine;

    public class SpriteChanger : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;  
        public Sprite[] sprites;             
        public float changeInterval = 0.3f;   

        private int _currentSpriteIndex;   
        private float _timer;               

        private void Start()
        {
            spriteRenderer.sprite = sprites[_currentSpriteIndex];
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= changeInterval)
            {
                _timer = 0f; 
                _currentSpriteIndex = (_currentSpriteIndex + 1) % sprites.Length;
                spriteRenderer.sprite = sprites[_currentSpriteIndex];
            }
        }
    }

}