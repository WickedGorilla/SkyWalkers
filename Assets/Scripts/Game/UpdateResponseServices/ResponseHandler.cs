using Infrastructure.Network;

namespace Game.UpdateResponseServices
{
    public abstract class ResponseHandler : IResponseHandler
    {
        protected ResponseHandler(IServerRequestSender serverRequestSender)
        {
            ServerRequestSender = serverRequestSender;
        }

        protected IServerRequestSender ServerRequestSender { get; }

        public abstract void StartListening();
        public abstract void StopListening();
    }
}