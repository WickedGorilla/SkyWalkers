using System;
using UnityEngine;

namespace Infrastructure.Animations
{
    public class AnimatorEntryStateChecker : StateMachineBehaviour
    {
        private Action _onEntry;
        private bool _isEntry;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isEntry = true;
            
            _onEntry?.Invoke();
            _onEntry = null;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
            => _isEntry = false;

        private void OnDisable()
            => _isEntry = false;

        public void SubscribeForEntry(Action onEntry)
        {
            if (_isEntry)
            {
                onEntry();
                return;
            }
            
            _onEntry = onEntry;
        }
    }
}