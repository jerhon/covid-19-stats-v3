using System;
using System.Linq;
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
        public void TestImportCovidData()
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
            
            var dataServiceMock = new Mock<ICovidTrackingDataService>();
            dataServiceMock.Setup((m) => m.GetDailyStateRecordsAsync()).ReturnsAsync(records.ToAsyncEnumerable());

            var dbContext = DbUtils.GetCovidDataSource();
            var import = new CovidTrackingProjectImport(dataServiceMock.Object, DbUtils.GetFactory());
            import.ImportAsync();

            Assert.AreEqual(2, dbContext.StatesDailyStatistics.Count());

            // re-importing the same data should keep # of records the same
            import.ImportAsync();
            
            Assert.AreEqual(2, dbContext.StatesDailyStatistics.Count());
        }
    }
}