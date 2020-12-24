using Honlsoft.CovidApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Honlsoft.CovidApp.Tests
{
    public class DbUtils
    {
        public static CovidDataContext GetCovidDataSource()
        {
            var loggerMock = new Mock<ILogger<CovidDataContext>>(); 
                             
            var options = new DbContextOptionsBuilder<CovidDataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryCovidDataContext")
                .Options;

            return new CovidDataContext(options, loggerMock.Object);
        }
        
        public static IDbContextFactory<CovidDataContext> GetFactory()
        {
            Mock<IDbContextFactory<CovidDataContext>> mock = new Mock<IDbContextFactory<CovidDataContext>>();
            mock.Setup((m) => m.CreateDbContext()).Returns(GetCovidDataSource());
            return mock.Object;
        }
        
        
    }
}