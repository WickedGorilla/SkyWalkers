using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CustomButtons
{
    public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isPressed = false;

        public event Action<Vector2> OnClicked;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isPressed)
                return;

            _isPressed = true;
            OnClicked?.Invoke(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }
        
    }
}