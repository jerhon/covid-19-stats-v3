using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Honlsoft.CovidApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Honlsoft.CovidApp.Controllers
{
    public record LatestNationDataDto(CovidNationDailyRecord DataPoint); 
    
    [ApiController]
    [Route("api/v1/nation/")]
    public class NationController : ControllerBase
    {
        #region Private Fields
        private readonly CovidDataContext _context;
        #endregion
        
        #region Ctor
        public NationController(CovidDataContext context)
        {
            _context = context;
        }
        #endregion
        
        [HttpGet("latest")]
        public async Task<LatestNationDataDto> GetLatest()
        {
            var latest = await _context.NationDailyStatistics.AsQueryable()
                .OrderByDescending((dp) => dp.Date)
                .FirstOrDefaultAsync();

            return new LatestNationDataDto(latest);
        }
    }
}