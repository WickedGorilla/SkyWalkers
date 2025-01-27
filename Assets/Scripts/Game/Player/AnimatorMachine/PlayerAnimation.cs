using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Disposables;
using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private readonly int _climbParamName = Animator.StringToHash("Climb");
        private readonly int _speedMultiplierParamName = Animator.StringToHash("Speed");

        private readonly LinkedList<Action> _onClimbListeners = new();

        [SerializeField] private Animator _animator;

        private Coroutine _currentCoroutine;
        
        public float DefaultSpeedMultiplier { get; private set; }
        public float SpeedMultiplier { get; private set; }

        private void Awake()
        {
            DefaultSpeedMultiplier = _animator.GetFloat(_speedMultiplierParamName);
            SpeedMultiplier = DefaultSpeedMultiplier;
        }

        public void AnimateByClick()
        {
            _animator.SetBool(_climbParamName, true);

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(AnimateCoroutine());
        }

        public void AnimateWhile(Func<bool> predicate)
        {
            _animator.SetBool(_climbParamName, true);
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                while (!predicate())
                    yield return null;

                _animator.SetBool(_climbParamName, false);
            }
        }

        private void Update()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.shortNameHash == _climbParamName)
                CallListeners();
        }

        private void CallListeners()
        {
            foreach (Action listener in _onClimbListeners)
                listener();
        }

        private IEnumerator AnimateCoroutine()
        {
            yield return new WaitForSeconds(0.2f);

            _animator.SetBool(_climbParamName, false);
            _currentCoroutine = null;
        }

        public IDisposable AddClimbListener(Action action)
        {
            _onClimbListeners.AddLast(action);
            return DisposableContainer.Create(() => _onClimbListeners.Remove(action));
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            SpeedMultiplier = multiplier;
            _animator.SetFloat(_climbParamName, SpeedMultiplier);
        }

        public void ResetSpeedMultiplier()
        {
            _animator.SetFloat(_climbParamName, DefaultSpeedMultiplier);
            SpeedMultiplier = DefaultSpeedMultiplier;
        }
    }
}