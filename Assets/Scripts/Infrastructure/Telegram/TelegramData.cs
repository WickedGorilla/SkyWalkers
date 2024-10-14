using System;

namespace Infrastructure.Telegram
{
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