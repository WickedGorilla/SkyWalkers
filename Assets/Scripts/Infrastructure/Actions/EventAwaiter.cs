using System;

namespace Infrastructure.Actions
{
    public class EventAwaiter : IEventAwaiter
    {
        private Action _action;

        private bool _isComplete;
        
        public void AddAwaiter(Action action)
        {
            if (_isComplete)
            {
                action();
                return;
            }
             
            _action = action;
        }

        public void Complete()
        {
            _isComplete = true;
            _action?.Invoke();
        }
    }
}