using System;

namespace Infrastructure.Disposables
{
    public class DisposableContainer : IDisposable
    {
        private readonly Action _disposeAction;

        private DisposableContainer(Action disposeAction) 
            => _disposeAction = disposeAction;

        public void Dispose() 
            => _disposeAction();
        
        public static DisposableContainer Create(Action disposeAction) 
            => new(disposeAction);
    }
}