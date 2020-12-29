using System;
using System.Data.Common;
using Honlsoft.CovidApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace Honlsoft.CovidApp.Tests
{
    /// <summary>
    /// Provides a simple harness for a test, the database will be attached only to this instance and be removed at
    /// the end of the test.
    /// </summary>
    public class DatabaseHarness: IDisposable
    {
        private readonly DbContextOptions<CovidDataContext> _databaseOptions;
        private readonly DbConnection _dbConnection;
        private bool _dbInitialized;

        public DatabaseHarness()
        {
            _dbConnection = SqlLiteUtils.CreateInMemoryDatabase();
            
            _databaseOptions = new DbContextOptionsBuilder<CovidDataContext>()
                .UseSqlite(_dbConnection)
                .Options;

            _dbInitialized = false;
        }
        
        public CovidDataContext GetCovidDataSource()
        {
            var loggerMock = new Mock<ILogger<CovidDataContext>>();
            var context = new CovidDataContext(_databaseOptions, loggerMock.Object);

            if (!_dbInitialized)
            {
                _dbInitialized = true;
                context.Database.EnsureCreated();
            }

            return context;
        }
        
        public IDbContextFactory<CovidDataContext> GetFactory()
        {
            Mock<IDbContextFactory<CovidDataContext>> mock = new Mock<IDbContextFactory<CovidDataContext>>();
            mock.Setup((m) => m.CreateDbContext()).Returns(this.GetCovidDataSource);
            return mock.Object;
        }

        /// <summary>
        /// Dispose of any disposable things it owns.
        /// </summary>
        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}