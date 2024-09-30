using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.CustomButtons
{
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _button;

        public string Text => _text.text;
        public Sprite Icon => _iconImage.sprite;
        public RectTransform Rect => transform as RectTransform;
        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }
        
        public void AddListener(UnityAction action) 
            => _button.onClick.AddListener(action);

        public void RemoveListener(UnityAction action) 
            => _button.onClick.RemoveListener(action);
    }
}