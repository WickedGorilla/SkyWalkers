using Game.Infrastructure;
using UnityEngine;
using Zenject;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private SceneContext _bootSceneContext;
        
    private void Start()
    {
        SceneContext bootContext = Instantiate(_bootSceneContext);
        bootContext.Run();
        bootContext.Container.Resolve<IGameStateMachine>().Enter<BootState>();
    }
}
