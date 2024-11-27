using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Infrastructure.Telegram
{
    public class TelegramLauncher : MonoBehaviour
    {
        private event Action OnCloseInvoice;

        public TelegramData TgData { get; private set; }

        public bool IsInit => TgData is not null;

        public string UserName
            => string.IsNullOrEmpty(TgData.username)
                ? $"{TgData.first_name} {TgData.last_name}"
                : TgData.username;

        public long UserId
            => TgData.id;

        public string PhotoUrl => TgData.photo_url;
        public long AuthDate => TgData.auth_date;
        public string Hash => TgData.hash;

        public string LanguageCode
            => string.IsNullOrEmpty(TgData.language_code) ? "en" : TgData.language_code;

        public bool IsLaunchedFromReferralUrl { get; private set; }
        public long ReferralCode { get; private set; }

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

        [DllImport("__Internal")]
        private static extern void OnUnityReady();

        // Call from Telegram .jsLib
        private void SetTelegramId(string jsonData)
        {
            TgData = JsonUtility.FromJson<TelegramData>(jsonData);

            if (TgData == null)
                Debug.LogError("TG data is null");
        }

        // Call from Telegram .jsLib
        private void SetReferralCode(string code)
        {
            IsLaunchedFromReferralUrl = true;
            ReferralCode = long.TryParse(code, out long refCode) ? refCode : 0;
        }

        public void OpenInvoiceLink(string url, Action action)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
         OnOpenInvoiceLink(url);
            OnCloseInvoice = action;
#endif
        }

        [DllImport("__Internal")]
        private static extern void OnOpenInvoiceLink(string url);

        // Call from Telegram .jsLib
        private void CloseInvoice()
        {
            OnCloseInvoice?.Invoke();
            OnCloseInvoice = null;
        }
    }
}