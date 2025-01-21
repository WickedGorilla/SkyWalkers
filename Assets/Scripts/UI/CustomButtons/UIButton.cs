using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CustomButtons
{
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private int _pointerId = -1;
        public bool IsPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsPressed && eventData.pointerId != _pointerId)
                return;

            _pointerId = eventData.pointerId;
            IsPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsPressed && eventData.pointerId != _pointerId)
                return;

            IsPressed = false;
            _pointerId = eventData.pointerId;
        }
    }
}