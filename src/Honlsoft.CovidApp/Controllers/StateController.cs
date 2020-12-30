using System;
using System.Collections.Generic;
using Honlsoft.CovidApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Honlsoft.CovidApp.Controllers
{
    public record GetStateDataDto(string Abbreviation, string Name, IEnumerable<CovidStateDailyRecord> DataPoints);

    public record AggregateDto(string Name, AggregateDataPointsDto State, AggregateDataPointsDto Total);

    public record AggregateDataPointsDto(int Positive, int Death);
    
    [ApiController]
    [Route("api/v1/states/")]
    public class StateController : ControllerBase
    {
        private readonly CovidDataContext _dataContext;
        private readonly ILogger<StateController> _logger;

        public StateController(CovidDataContext dataContext, ILogger<StateController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet("{stateAbbreviation}")]
        public async Task<GetStateDataDto> GetStateData(string stateAbbreviation)
        {
            var state = await _dataContext.States.AsQueryable().FirstOrDefaultAsync((s) => s.Abbreviation == stateAbbreviation);
            var dataPoints = await _dataContext.StatesDailyStatistics.AsQueryable().Where((s) => s.State == stateAbbreviation).ToArrayAsync();
            var dto = new GetStateDataDto(state?.Abbreviation, state?.Name, dataPoints);
            return dto;
        }

        [HttpGet("{stateAbbreviation}/aggregate")]
        public async Task<AggregateDto> GetAggregate(string stateAbbreviation, DateTime? since)
        {
            since ??= DateTime.Now.AddDays(-7);
            
            var state = await _dataContext.States.AsQueryable().FirstOrDefaultAsync((s) =>
                s.Abbreviation == stateAbbreviation);

            var last7Days = _dataContext.StatesDailyStatistics.AsQueryable()
                .Where((s) => s.Date >= since)
                .Select((dp) => new { Death = dp.Death ?? 0, Positive = dp.Positive ?? 0});
            
            var totalPositive = await last7Days.MaxAsync((d) => d.Positive) - await last7Days.MinAsync((d) => d.Positive); 
            var totalDeaths = await last7Days.MaxAsync((d) => d.Death) - await last7Days.MinAsync((d) => d.Death);
            var total = new AggregateDataPointsDto(totalPositive, totalDeaths);

            var aggregate = await _dataContext.StatesDailyStatistics.AsQueryable()
                .Where((s) => s.Date >= since && s.State == stateAbbreviation)
                .Select((dp) => new { State = dp.State, Death = dp.Death ?? 0, Positive = dp.Positive ?? 0 })
                .GroupBy((dp) => dp.State)
                .Select((dp) => new AggregateDataPointsDto(
                    dp.Max((r) => r.Positive) - dp.Min((r) => r.Positive), 
                    dp.Max((r) => r.Death) - dp.Min((r) => r.Death)))
                .FirstOrDefaultAsync();
            
            return new AggregateDto(state.Name, aggregate, total);
        }
    }
}