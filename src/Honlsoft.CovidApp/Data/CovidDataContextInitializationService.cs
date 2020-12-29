using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Honlsoft.CovidApp.Data
{
    public class CovidDataContextInitializationService : IHostedService
    {
        private readonly IDbContextFactory<CovidDataContext> _factory;
        
        public CovidDataContextInitializationService(IDbContextFactory<CovidDataContext> factory)
        {
            _factory = factory;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var context = _factory.CreateDbContext();
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
