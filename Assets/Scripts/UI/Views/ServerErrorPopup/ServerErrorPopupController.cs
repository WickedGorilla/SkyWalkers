using System;
using Infrastructure.Network;
using Infrastructure.Network.Request.UpdateGameData;
using UI.Core;

namespace UI.Views.ServerErrorPopup
{
    public class ServerErrorPopupController : ViewController<ServerErrorPopupView>
    {
        private readonly IServerRequestSender _serverRequestSender;

        private IDisposable _viewDisposable;

        public ServerErrorPopupController(ServerErrorPopupView view,
            IServerRequestSender serverRequestSender) : base(view)
        {
            _serverRequestSender = serverRequestSender;
        }

        public void Initialize(int errorCode) 
            => _viewDisposable = View.Initialize(errorCode, OnSendUpdateGameData);

        protected override void OnHide() 
            => _viewDisposable?.Dispose();
        
        private void OnSendUpdateGameData()
        {
            _serverRequestSender.SendToServerAndHandle<UpdateGameDataRequest, 
                UpdateGameDataResponse>(UpdateGameDataRequest.Empty, 
                ServerAddress.UpdateGameStateAfterError,
                OnComplete);

            View.ShowLoader();
            
            void OnComplete(ServerResponse<UpdateGameDataResponse> serverResponse)
            {
                View.HideLoader();
                Hide();
            }
        }
    }
}