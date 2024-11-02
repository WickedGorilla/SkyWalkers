using System.Runtime.InteropServices;
using UnityEngine;

namespace Infrastructure.Telegram
{
    public class TelegramLauncher : MonoBehaviour
    {
        private TelegramData _tgData;

        public bool IsInit => _tgData is not null;

        public string UserName
            => string.IsNullOrEmpty(_tgData.username)
                ? $"{_tgData.first_name} {_tgData.last_name}"
                : _tgData.username;

        public long UserId
            => _tgData.id;

        public string PhotoUrl => _tgData.photo_url;
        public long AuthDate => _tgData.auth_date;
        public string Hash => _tgData.hash;

        private void Awake()
            => DontDestroyOnLoad(gameObject);

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        OnUnityReady();
#elif UNITY_EDITOR

            var data = new TelegramData
            {
                username = "Balagun",
                photo_url =
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS22mp7-FVbNvrh9ZM8bHX_9VKmr7zMay3-6g&s",
                id = 101
            };

            SetTelegramId(JsonUtility.ToJson(data));
#endif
        }

        // Call from Telegram .jsLib
        public void SetTelegramId(string jsonData)
        {
            _tgData = JsonUtility.FromJson<TelegramData>(jsonData);

            if (_tgData == null)
                Debug.LogError("TG data is null");
        }


        [DllImport("__Internal")]
        private static extern void OnUnityReady();
    }
}