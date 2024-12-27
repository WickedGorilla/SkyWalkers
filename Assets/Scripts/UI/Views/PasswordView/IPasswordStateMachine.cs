using System.Collections.Generic;

namespace UI.Views
{
    public interface IPasswordStateMachine
    {
        void EnterState<TState>(LinkedList<int> selectedNodes) where TState : PasswordState;
    }
}