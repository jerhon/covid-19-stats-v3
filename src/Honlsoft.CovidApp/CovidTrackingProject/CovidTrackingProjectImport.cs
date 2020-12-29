using System.Linq;
using System.Threading.Tasks;
using Honlsoft.CovidApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Honlsoft.CovidApp.CovidTrackingProject
{
    public class CovidTrackingProjectImport
    {
        private readonly ICovidTrackingDataService _dataService;
        private readonly IDbContextFactory<CovidDataContext> _dataContextFactory;

        public CovidTrackingProjectImport(ICovidTrackingDataService dataService, IDbContextFactory<CovidDataContext> dataContextFactory)
        {
            _dataService = dataService;
            _dataContextFactory = dataContextFactory;
        }

        public async Task ImportNationsAsync()
        {
            await using var context = _dataContextFactory.CreateDbContext();
            var records = await _dataService.GetDailyNationRecordsAsync();
            await foreach (var record in records)
            {
                var dataPoint = await context.NationDailyStatistics.AsQueryable().FirstOrDefaultAsync((dp) => dp.Date == record.Date);
                if (dataPoint?.SourceHash?.SequenceEqual(record.SourceHash) ?? false)
                {
                    continue;
                }
                if (dataPoint != null)
                {
                    context.Remove(dataPoint);
                }
                context.Add(record);
            }

            await context.SaveChangesAsync();
        }
        
        public async Task ImportStatesAsync()
        {
            await using var context = _dataContextFactory.CreateDbContext();
            var records = await _dataService.GetDailyStateRecordsAsync();
            await foreach (var @new in records)
            {
                var state = await context.StatesDailyStatistics.AsQueryable()
                    .FirstOrDefaultAsync((s) => s.Date == @new.Date && s.State == @new.State);
                
                if (state?.SourceHash?.SequenceEqual(@new.SourceHash) ?? false)
                {
                    continue;
                }
                if (state != null)
                {
                    context.Remove(state);
                }
                context.Add(@new);
            }
            await context.SaveChangesAsync();
        }
    }
}