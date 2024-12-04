using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CustomButtons
{
    public class TouchArea : MonoBehaviour, IPointerDownHandler
    {
        public event Action<Vector2> OnClicked;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClicked?.Invoke(eventData.position);
        }
    }
}