using UnityEngine;

namespace UI.Core
{
    [RequireComponent(typeof(Canvas))]
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private RectTransform _layer1;
        [SerializeField] private RectTransform _layer2;
        
        public RectTransform Layer1 => _layer1;
        public RectTransform Layer2 => _layer2;
    }
}