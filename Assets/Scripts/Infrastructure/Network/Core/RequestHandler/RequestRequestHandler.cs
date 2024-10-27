using System.Collections.Generic;

namespace Infrastructure.Network.RequestHandler
{
    public class RequestRequestHandler<T> : IRequestHandlersHolder<T>
    {
        private HashSet<IRequestHandler<T>> _handlers = new();
        
        public void Handle(T response)
        {
            foreach (var handler in _handlers) 
                handler.Handle(response);
        }

        public void Add(IRequestHandler<T> requestHandler) 
            => _handlers.Add(requestHandler);

        public void Remove(IRequestHandler<T> requestHandler) 
            => _handlers.Remove(requestHandler);
    }
}