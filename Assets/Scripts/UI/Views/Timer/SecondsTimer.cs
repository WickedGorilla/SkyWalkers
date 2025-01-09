using System;
using System.Threading;
using UnityEngine;

namespace UI.Views.Timer
{
    public class SecondsTimer : IUpdateTimer
    {
        private readonly int _seconds;
        private readonly Action<int> _onUpdate;
        private readonly Action _onComplete;
        private CancellationTokenSource _cancellationTokenSource;

        public SecondsTimer(int seconds, Action<int> onUpdate, Action onComplete)
        {
            _seconds = seconds;
            _onUpdate = onUpdate;
            _onComplete = onComplete;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            for (int i = _seconds; i > 0; i--)
            {
                if (token.IsCancellationRequested)
                    return;

                _onUpdate(i);
                await Awaitable.WaitForSecondsAsync(1f);
            }

            if (token.IsCancellationRequested) 
                return;
            
            _onUpdate(0);
            _onComplete();
        }

        public void Stop() 
            => _cancellationTokenSource.Cancel();
    }
}