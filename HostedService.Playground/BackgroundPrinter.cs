using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedService.Playground
{
    public class BackgroundPrinter : IHostedService, IDisposable
    {
        private readonly ILogger<BackgroundPrinter> _logger;
        private int _number = 0;
        private Timer _timer;

        public BackgroundPrinter(ILogger<BackgroundPrinter> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(o =>
                {
                    Interlocked.Increment(ref _number);
                    _logger.LogInformation($"Printing from worker number {_number}");
                }, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Printing worker stopping");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
