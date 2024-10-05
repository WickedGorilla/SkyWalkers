mergeInto(LibraryManager.library, {
    OnUnityReady: function () {


        window.Telegram.WebApp.ready();

        const user = window.Telegram.WebApp.initDataUnsafe.user;
        const queryId = window.Telegram.WebApp.initDataUnsafe.query_id;

      window.unityInstance.SendMessage('TelegramLauncher', 'SetTelegramId', 'kkkjjkjk');


        if (user) {
            const telegramData = {
                id: user.id,
                first_name: user.first_name,
                last_name: user.last_name,
                username: user.username,
                photo_url: user.photo_url,
                auth_date: window.Telegram.WebApp.initDataUnsafe.auth_date,
                hash: window.Telegram.WebApp.initDataUnsafe.hash
            };

            const telegramDataString = JSON.stringify(telegramData);

            function sendMessageToUnity() {
                if (window.unityInstance && window.unityInstance.SendMessage) {
                    window.unityInstance.SendMessage('TelegramLauncher', 'SetTelegramId', 'kkkjjkjk');
                } else {
 console.error("Unity не загружен.");
                    setTimeout(sendMessageToUnity, 1000);
                }
            }

            sendMessageToUnity();
        }
    }
});