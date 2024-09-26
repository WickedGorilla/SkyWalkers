using Game.Infrastructure;

namespace Game.Infrastructure
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
    }
}