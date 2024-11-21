using Infrastructure.Network;

namespace Game.UpdateResponseServices
{
    public abstract class ResponseHandler : IResponseHandler
    {
        protected ResponseHandler(ServerRequestSender serverRequestSender)
        {
            ServerRequestSender = serverRequestSender;
        }

        protected ServerRequestSender ServerRequestSender { get; }

        public abstract void StartListening();
        public abstract void StopListening();
    }
}