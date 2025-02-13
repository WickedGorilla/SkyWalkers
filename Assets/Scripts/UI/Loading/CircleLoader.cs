using DG.Tweening;
using UnityEngine;

namespace UI.Loading
{
    public class CircleLoader : MonoBehaviour
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private float _duration = 1f;
        
        private void OnEnable() 
            => RotateLoader(_transform);

        private void OnDisable() 
            => _transform.DOKill();

        private void RotateLoader(RectTransform transform)
        {
            transform.DORotate(new Vector3(0, 0, -360f), _duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }
    }
}