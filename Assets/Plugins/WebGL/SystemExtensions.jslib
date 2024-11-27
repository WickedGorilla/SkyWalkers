mergeInto(LibraryManager.library, {
    OnCopyWebGLText: function (textPtr) {
        var text = UTF8ToString(textPtr);
        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(text).then(function () {
                console.log("Текст скопирован: " + text);
            }).catch(function (err) {
                console.error("Ошибка копирования: ", err);
            });
        } else {
            var textArea = document.createElement("textarea");
            textArea.value = text;
            document.body.appendChild(textArea);
            textArea.select();
            try {
                document.execCommand("copy");
                console.log("Текст скопирован (fallback): " + text);
            } catch (err) {
                console.error("Ошибка копирования (fallback): ", err);
            }
            document.body.removeChild(textArea);
        }
    },

    OpenUrlLinkWebGL: function (urlPtr) {
        var url = UTF8ToString(urlPtr);

        if (window.Telegram && window.Telegram.WebApp) {

   window.Telegram.WebApp.openInvoice(url);

    window.Telegram.WebApp.onEvent('invoiceClosed', function () {
        console.log("Платеж завершен.");
        if (window.unityInstance) {
            window.unityInstance.SendMessage("WebGLEvent", "OnCurrentPageClose");
        }
    });



    }
    }
});
