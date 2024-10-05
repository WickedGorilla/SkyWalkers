using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Infrastructure.Telegram
{
    public class TelegramLauncher : MonoBehaviour
    {
        private TelegramData _tgData;
        
        public bool IsInit => !(_tgData is null);
        public string UserId => _tgData.username;
        public string PhotoUrl => _tgData.photo_url;
        
        private void Awake() 
            => DontDestroyOnLoad(gameObject);

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        OnUnityReady();
#endif
        }

        // Call from Telegram .jsLib
        public void SetTelegramId(string jsonData)
        {
            _tgData = JsonUtility.FromJson<TelegramData>(jsonData);

            if (_tgData != null)
                return;
            
            Debug.Log("TG data is null:");
        }
        
        [DllImport("__Internal")]
        private static extern void OnUnityReady();
        
        [Serializable]
        public class TelegramData
        {
            public int id;
            public string first_name;
            public string last_name;
            public string username;
            public string photo_url;
            public long auth_date;
            public string hash;
        }
    }
}