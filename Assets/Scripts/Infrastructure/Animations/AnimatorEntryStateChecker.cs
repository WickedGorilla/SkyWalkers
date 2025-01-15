using System;
using UnityEngine;

namespace Infrastructure.Animations
{
    public class AnimatorEntryStateChecker : StateMachineBehaviour
    {
        private Action _onEntry;
        
        public bool IsEntry { get; private set; }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            IsEntry = true;

            _onEntry?.Invoke();
            _onEntry = null;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
            => IsEntry = false;

        public void SubscribeForEntry(Action onEntry)
        {
            if (IsEntry)
            {
                onEntry();
                return;
            }
            
            _onEntry = onEntry;
        }
    }
}