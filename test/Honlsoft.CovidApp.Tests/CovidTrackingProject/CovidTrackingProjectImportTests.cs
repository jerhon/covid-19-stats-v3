using System;
using System.Linq;
using System.Threading.Tasks;
using Honlsoft.CovidApp.CovidTrackingProject;
using Honlsoft.CovidApp.Data;
using Moq;
using NUnit.Framework;

namespace Honlsoft.CovidApp.Tests.CovidTrackingProject
{
    [TestFixture]
    public class CovidTrackingProjectImportTests
    {
        [Test]
        public async Task TestImportCovidData()
        {
            CovidStateDailyRecord[] records =
            {
                new()
                {
                    Date = DateTime.Today,
                    State = "ND",
                    SourceHash = new byte[] {1, 2, 3, 4}
                },
                new()
                {
                    Date = DateTime.Today.AddDays(-1),
                    State = "ND",
                    SourceHash = new byte[] {5, 6, 7, 8}
                }
            };
            var dbHarness = new DatabaseHarness();
            
            var dataServiceMock = new Mock<ICovidTrackingDataService>();
            dataServiceMock.Setup((m) => m.GetDailyStateRecordsAsync()).ReturnsAsync(records.ToAsyncEnumerable());

            var dbContext = dbHarness.GetCovidDataSource();
            var import = new CovidTrackingProjectImport(dataServiceMock.Object, dbHarness.GetFactory());
            await import.ImportStatesAsync();

            Assert.AreEqual(2, dbContext.StatesDailyStatistics.Count());

            // re-importing the same data should keep # of records the same
            await import.ImportStatesAsync();
            
            Assert.AreEqual(2, dbContext.StatesDailyStatistics.Count());
        }
    }
}