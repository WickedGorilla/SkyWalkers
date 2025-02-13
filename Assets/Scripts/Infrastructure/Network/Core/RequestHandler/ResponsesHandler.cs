using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Network.RequestHandler
{
    public class ResponsesHandler
    {
        private readonly Dictionary<Type, object> _handlers = new();
        
        public IRequestHandlersHolder<T>? GetHolder<T>()
        {
            if (_handlers.TryGetValue(typeof(T), out object handler) 
                && handler is IRequestHandlersHolder<T> typedHandler)
            {
                return typedHandler;
            }

            return null;
        }
        
        public void AddHandlers<T>(params IRequestHandler<T>[] handlers)
        {
            var handlersHolders = GetHolder<T>();

            if (handlersHolders is null)
            {
                handlersHolders = new RequestHandler<T>();
                AddHolder(handlersHolders);
            }
            
            foreach (var handler in handlers) 
                handlersHolders.Add(handler);
        }
        
        public void RemoveHandlers<T>(params IRequestHandler<T>[] handlers)
        {
            var holder = GetHolder<T>();

            if (holder is null)
            {
                Debug.LogError($"No holder by type {typeof(T)}");
                return;
            }
            
            foreach (var handler in handlers) 
                holder.Remove(handler);
        }
        
        private void AddHolder<T>(IRequestHandlersHolder<T> holder) 
            => _handlers[typeof(T)] = holder;
    }
}