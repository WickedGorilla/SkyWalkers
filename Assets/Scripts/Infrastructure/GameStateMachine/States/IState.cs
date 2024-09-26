namespace Game.Infrastructure
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}