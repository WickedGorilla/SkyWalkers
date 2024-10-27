namespace Infrastructure.Network.RequestHandler
{
    public interface IRequestHandler<in T>
    {
        void Handle(T response);
    }
}