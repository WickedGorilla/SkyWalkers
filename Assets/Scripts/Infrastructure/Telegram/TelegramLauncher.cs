using System.Runtime.InteropServices;
using UnityEngine;

namespace Infrastructure.Telegram
{
    public class TelegramLauncher : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void OnUnityReady();
        
        public bool IsInit { get; private set; }
        public string UserId { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        OnUnityReady();
#endif
        }

        // Call from Telegram index.html
        public void SetTelegramId(string userId)
        {
            UserId = userId;
            IsInit = true;
            Debug.Log("TG ID ready:" + userId);
        }
    }
}