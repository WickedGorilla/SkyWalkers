using DG.Tweening;
using UnityEngine;

public class CircleAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform[] _transforms;
    [SerializeField] private float _duration = 3f;

    private static readonly Vector3 _zRotate = new(0f, 0f, 360f);
    
    private void OnEnable()
    {
        foreach (RectTransform trans in _transforms)
            DoCycleRotateZ(trans);
    }

    private void OnDisable()
    {
        foreach (var trans in _transforms)
            trans.DOKill();
    }
    
    private void DoCycleRotateZ(RectTransform transform)
    {
        transform.DOLocalRotate(_zRotate, _duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
