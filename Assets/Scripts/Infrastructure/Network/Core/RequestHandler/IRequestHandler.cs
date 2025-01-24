namespace Infrastructure.Network.RequestHandler
{
    public interface IRequestHandler<in T>
    {
        void HandleServerData(T response);
    }
}