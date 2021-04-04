using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CryptocurrencyTracker.Services
{
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task _backgroundTask;
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _backgroundTask = ExecuteAsync(_cancelTokenSource.Token);
            if (_backgroundTask.IsCompleted)
                return _backgroundTask;
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_backgroundTask == null)
                return;

            try { _cancelTokenSource.Cancel(); }
            finally
            {
                Debug.WriteLine("IHostedService::StopAsync()");
                await Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
            Debug.WriteLine("IHostedService::StopAsync::End()");
        }

        public virtual void Dispose()
        {
            Debug.WriteLine("IHostedService::Dispose()");
            _cancelTokenSource.Cancel();
        }
    }
}