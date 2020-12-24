using System.IO;
using System.Net;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Honlsoft.CovidApp.Data
{
    public class CovidDataContext: DbContext
    {
        private readonly ILogger<CovidDataContext> _logger;

        public CovidDataContext(DbContextOptions<CovidDataContext> options, ILogger<CovidDataContext> logger): base(options)
        {
            _logger = logger;
        }
        
        public DbSet<CovidStateDailyRecord> StatesDailyStatistics { get; set; }
        
        public DbSet<CovidNationDailyRecord> NationDailyStatistics { get; set; }
        
        public DbSet<State> States { get; set; }

        private void InitTableData<TSeedEntity>(ModelBuilder builder) where TSeedEntity: class
        {
            var fileName = typeof(TSeedEntity).Name + ".json";
            builder.Entity<TSeedEntity>().HasData(DbSetExtensions.LoadDataSeed<TSeedEntity[]>(fileName));
            _logger.LogDebug("Loaded seed data from: {0}", fileName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            InitTableData<State>(modelBuilder);
        }
    }
}