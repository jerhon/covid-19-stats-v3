using System.Collections.Generic;
using System.Threading.Tasks;
using Honlsoft.CovidApp.Data;

namespace Honlsoft.CovidApp.CovidTrackingProject
{
    public interface ICovidTrackingDataService
    {
        Task<IAsyncEnumerable<CovidStateDailyRecord>> GetDailyStateRecordsAsync();
        Task<IAsyncEnumerable<CovidNationDailyRecord>> GetDailyNationRecordsAsync();
    }
}