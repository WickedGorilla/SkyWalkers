using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Disposables;
using UnityEngine;

namespace UI.CustomButtons
{
    public class SwitchButtons : MonoBehaviour
    {
        [SerializeField] private SwitchButton[] _buttons;
        [SerializeField] private ButtonSelector _selectableLabel;

        private IEnumerable<IDisposable> _disposables; 
        
        private SwitchButton _currentClicked;
        private bool _isMoved;
        
        private void OnEnable()
        {
            var disposables = new LinkedList<IDisposable>();
            
            foreach (var button in _buttons)
            {
                button.AddListener(RegisterButton);

                disposables.AddLast(DisposableContainer.Create(
                    () => button.RemoveListener(RegisterButton)));

                void RegisterButton() 
                    => OnClickButton(button);
            }

            _disposables = disposables;
        }

        private void OnDisable()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
        
        private void OnClickButton(SwitchButton button)
        {
            if (_isMoved)
                return;
            
            if (_currentClicked != null)
                _currentClicked.Interactable = true;
                
            _currentClicked = button;
            _currentClicked.Interactable = false;
            AnimatedMove(_currentClicked);
        }

        private void AnimatedMove(SwitchButton button)
        {
            _isMoved = true;
            
            _selectableLabel.Rect.DOMoveX(button.Rect.position.x, 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => _isMoved = false);
            
            _selectableLabel.Select(button);
        }
    }
}