using System;

namespace Infrastructure.Actions
{
    public interface IEventAwaiter
    {
        void AddAwaiter(Action action);
    }
}