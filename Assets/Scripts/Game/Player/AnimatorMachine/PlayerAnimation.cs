using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Disposables;
using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private readonly int _climbName = Animator.StringToHash("Climb");
        private readonly LinkedList<Action> _onClimbListeners = new();
        
        [SerializeField] private Animator _animator;
        
        private Coroutine _currentCoroutine;
        
        public void AnimateByClick()
        {
            _animator.SetBool(_climbName, true);

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(AnimateCoroutine());
        }

        public void AnimateWhile(Func<bool> predicate)
        {
            _animator.SetBool(_climbName, true);
            StartCoroutine(Routine());
            
            IEnumerator Routine()
            {
                while (!predicate())
                    yield return null;
                
                _animator.SetBool(_climbName, false);
            }
        }
        
        private void Update()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.shortNameHash == _climbName)
                CallListeners();
        }

        private void CallListeners()
        {
            foreach (var listener in _onClimbListeners)
                listener();
        }

        private IEnumerator AnimateCoroutine()
        {
            yield return new WaitForSeconds(0.2f);

            _animator.SetBool(_climbName, false);
            _currentCoroutine = null;
        }

        public IDisposable AddClimbListener(Action action)
        {
            _onClimbListeners.AddLast(action);
            return DisposableContainer.Create(() => _onClimbListeners.Remove(action));
        }
    }
}