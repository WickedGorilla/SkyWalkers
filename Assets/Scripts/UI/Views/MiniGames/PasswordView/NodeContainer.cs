using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    [Serializable]
    public class NodeContainer
    {
        [SerializeField] private RectTransform _circle;
        [SerializeField] private Image _coloreImage;
        
        public Vector2 Position => Circle.localPosition;
        public RectTransform Circle => _circle;

        public void SetColor(Color color)
            => _coloreImage.color = color;
    }
}