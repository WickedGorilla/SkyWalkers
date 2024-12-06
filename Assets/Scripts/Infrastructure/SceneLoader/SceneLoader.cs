using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneManagement
{
    public class SceneLoader
    {
        public async Awaitable LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) 
            => await SceneManager.LoadSceneAsync(sceneName, mode);
    }
}