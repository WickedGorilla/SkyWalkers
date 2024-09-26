using UnityEngine;

namespace UI.Scalers
{
    public class BackgroundScaler : MonoBehaviour
    {
        [SerializeField] private Vector2 _defaultSpriteResolution = new(1920f, 1920f);

        private void Start()
        {
            if (Screen.width > Screen.height)
                ScaleByWidth();
            else
                ScaleByHeight();
        }

        private void ScaleByWidth()
        {
            var rectTransform = (RectTransform)transform;

            float currentRatio = Screen.width / _defaultSpriteResolution.x / rectTransform.lossyScale.x;
            rectTransform.sizeDelta *= currentRatio;
        }

        private void ScaleByHeight()
        {
            var rectTransform = (RectTransform)transform;
            float currentRatio = Screen.height / _defaultSpriteResolution.y / rectTransform.lossyScale.y;
            rectTransform.sizeDelta *= currentRatio;
        }
    }
}