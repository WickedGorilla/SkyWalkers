namespace Game.UpdateResponseServices
{
    public interface IResponseHandler
    {
        void StartListening();
        void StopListening();
    }
}