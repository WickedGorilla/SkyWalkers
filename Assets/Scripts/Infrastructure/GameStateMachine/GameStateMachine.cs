using Zenject;

namespace Game.Infrastructure
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly DiContainer _diContainer;

        private IState _currentState;
        
        public GameStateMachine(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Enter<TState>() where TState : class, IState
        {
            _currentState?.Exit();
            _currentState = _diContainer.Instantiate<TState>();
            _currentState.Enter();
        }
    }
}
