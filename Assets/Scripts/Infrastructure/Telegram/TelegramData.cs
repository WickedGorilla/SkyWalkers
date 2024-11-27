using System;

namespace Infrastructure.Telegram
{
    [Serializable]
    public class TelegramData
    {
        public long id;
        public string first_name;
        public string last_name;
        public string username;
        public string photo_url;
        public long auth_date;
        public string hash;
        public string language_code;
        public string InitData;
    }
}