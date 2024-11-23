if (typeof document !== 'undefined') 
{
   document.addEventListener("DOMContentLoaded", () => {
    handleReferralCode();});
}


function handleReferralCode() {
    const referralCode = getUrlParameter("startapp");
    if (referralCode) {
        // Ждем, пока Unity WebGL загрузится
        const waitForUnity = setInterval(() => {
            if (typeof unityInstance !== "undefined") {
                unityInstance.SendMessage("TelegramLauncher", "SetReferralCode", referralCode);
                clearInterval(waitForUnity);
            }
        }, 100);
    }
}

function getUrlParameter(parameterName) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(parameterName);
}
