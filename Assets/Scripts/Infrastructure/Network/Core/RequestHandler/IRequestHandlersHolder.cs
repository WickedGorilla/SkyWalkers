namespace Infrastructure.Network.RequestHandler
{
    public interface IRequestHandlersHolder<T> : IRequestHandler<T>
    {
        void Add(IRequestHandler<T> requestHandler);
        void Remove(IRequestHandler<T> requestHandler);
    }
}