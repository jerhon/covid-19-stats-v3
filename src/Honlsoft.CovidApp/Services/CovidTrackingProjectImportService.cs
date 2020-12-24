using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Honlsoft.CovidApp.CovidTrackingProject;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Honlsoft.CovidApp.Services
{
    public class CovidTrackingProjectImportService : IHostedService
    {
        private readonly Timer _timer;
        private readonly CovidTrackingProjectImport _import;
        private readonly ILogger<CovidTrackingProjectImportService> _logger;

        public CovidTrackingProjectImportService(CovidTrackingProjectImport import, ILogger<CovidTrackingProjectImportService> logger)
        {
            _timer = new();
            _timer.Interval = 60 * 60 * 1000;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Elapsed += TimerOnElapsed;

            _import = import;
            _logger = logger;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // On startup, import everything
            await UpdateAsync();
        }

        private async Task UpdateAsync()
        {
            try
            {
                _logger.LogInformation("Refreshing data from COVID tracking project.");
                _logger.LogInformation("Refreshing nation data.");
                await _import.ImportNationsAsync();
                _logger.LogInformation("Refreshing state data.");
                await _import.ImportStatesAsync();
                _logger.LogInformation("Done refreshing data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh data from COVID tracking project.  Will attempt again later.");
            }
        }

        private async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            await UpdateAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            return Task.CompletedTask;
        }
    }
}