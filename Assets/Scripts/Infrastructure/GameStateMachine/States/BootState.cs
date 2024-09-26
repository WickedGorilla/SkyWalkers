using Cysharp.Threading.Tasks;

namespace Game.Infrastructure
{
    public class BootState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly LoadingCurtain _loadingCurtain;

        public BootState(IGameStateMachine gameStateMachine, 
            LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;
        }
        
        public async void Enter()
        {
            _loadingCurtain.Show();
            
            await UniTask.DelayFrame(1);
            _gameStateMachine.Enter<BindGameState>();
        }

        public void Exit()
        {
         
        }
    }
}