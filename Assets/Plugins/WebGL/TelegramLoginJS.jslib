mergeInto(LibraryManager.library, {
    OnUnityReady: function () {

        window.Telegram.WebApp.ready();

        const user = window.Telegram.WebApp.initDataUnsafe.user;
        const queryId = window.Telegram.WebApp.initDataUnsafe.query_id;
 const initData = window.Telegram.WebApp.initData;


        if (user) {
            const telegramData = 
{
                id: user.id,
                first_name: user.first_name,
                last_name: user.last_name,
                username: user.username,
                photo_url: user.photo_url,
                auth_date: window.Telegram.WebApp.initDataUnsafe.auth_date,
                hash: window.Telegram.WebApp.initDataUnsafe.hash,
language_code:  window.Telegram.WebApp.initDataUnsafe.language_code,
InitData: initData
            };

            const jsonData = JSON.stringify(telegramData);

            function sendMessageToUnity() {
                if (window.unityInstance && window.unityInstance.SendMessage) {
                    window.unityInstance.SendMessage('TelegramLauncher', 'SetTelegramId', jsonData);
                } else {
 console.error("Engine dont load.");
                    setTimeout(sendMessageToUnity, 1000);
                }
            }

            sendMessageToUnity();
        }
    },


   OnOpenInvoiceLink: function (urlPtr) {
        var url = UTF8ToString(urlPtr);

        if (window.Telegram && window.Telegram.WebApp) {

   window.Telegram.WebApp.openInvoice(url);

    window.Telegram.WebApp.onEvent('invoiceClosed', function () {
              if (window.unityInstance) {
            window.unityInstance.SendMessage('TelegramLauncher', 'CloseInvoice');
        }
    });
    }
    }



});