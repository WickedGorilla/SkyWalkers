using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomButtons
{
    public class ButtonSelector : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _iconImage;
        public RectTransform Rect => transform as RectTransform;

        public void Select(SwitchButton switchButton)
        {
            _text.text = switchButton.Text;
            _iconImage.sprite = switchButton.Icon;
        }
    }
}