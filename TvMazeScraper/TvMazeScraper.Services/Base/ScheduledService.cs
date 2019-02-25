using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TvMazeScraper.Services.Base
{
    public abstract class ScheduledService<T> : BackgroundService where T : class
    {
        protected readonly ILogger<T> Logger;
        
        protected ScheduledService(ILogger<T> logger)
        {
            Logger = logger;
        }

        protected TimeSpan DelayTimeSpan { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Started {Service} at {Time}.", typeof(T).Name, DateTime.UtcNow);

            stoppingToken.Register(() =>
                Logger.LogInformation("Stopped {Service} at {Time}.", typeof(T).Name, DateTime.UtcNow));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWorkAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                    Logger.LogError(e.StackTrace);
                }

                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
        }

        public abstract Task DoWorkAsync(CancellationToken stoppingToken);
    }
}