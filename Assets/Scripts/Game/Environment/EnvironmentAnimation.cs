using Infrastructure.Animations;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private int _showAnimationHash;
        private AnimatorEntryStateChecker _emptyStateChecker;

        private const string ShowAnimation = "Show";
        
        private void Awake()
        {
            _showAnimationHash = Animator.StringToHash(ShowAnimation);
            _emptyStateChecker = _animator.GetBehaviour<AnimatorEntryStateChecker>();

            _animator.SetBool(_showAnimationHash, false);
        }

        public void DoShow()
        {
            gameObject.SetActive(true);
            _animator.SetBool(_showAnimationHash, true);
        }

        public void DoHide()
        {
            _animator.SetBool(_showAnimationHash, false);
            _emptyStateChecker.SubscribeForEntry((
                ) => gameObject.SetActive(false));
        }
    }
}
