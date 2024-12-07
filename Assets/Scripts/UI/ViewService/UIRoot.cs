using UnityEngine;

namespace UI.Core
{
    [RequireComponent(typeof(Canvas))]
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private RectTransform _layer1;
        [SerializeField] private RectTransform _layer2Permanent;
        [SerializeField] private RectTransform _layer3Popup;
        
        public RectTransform Layer1 => _layer1;
        public RectTransform Layer2Permanent => _layer2Permanent;
        public RectTransform Layer3Popup => _layer3Popup;
    }
}