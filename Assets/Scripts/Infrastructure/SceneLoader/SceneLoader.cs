using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneManagement
{
    public class SceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) 
            => await SceneManager.LoadSceneAsync(sceneName, mode);
    }
}