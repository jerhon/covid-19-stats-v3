using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Honlsoft.CovidApp.CovidTrackingProject;
using NUnit.Framework;
using System.Threading.Tasks;
using Honlsoft.CovidApp.Data;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.Common;
using Moq;

namespace Honlsoft.CovidApp.Tests.CovidTrackingProject
{
    [TestFixture]
    public class CovidTrackingDataServiceTests
    {
        [Test]
        public async Task ParseCovidData()
        {
            var text = await File.ReadAllTextAsync(Path.Join(TestContext.CurrentContext.TestDirectory,
                "CovidTrackingProject", "all-states-history.csv"));
            var records = await CovidTrackingDataService.ParseCsvRecords<CovidStateDailyRecord>(new StringReader(text))
                .ToArrayAsync();
            Assert.AreEqual(14279, records.Length);

            // records are line # - 2
            var nd11122020 = records[31];

            Assert.AreEqual("ND", nd11122020.State);
            Assert.AreEqual(DateTime.Parse("11-12-2020"), nd11122020.Date);
            Assert.AreEqual("B", nd11122020.DataQualityGrade);
            Assert.AreEqual(553, nd11122020.Death);
            Assert.AreEqual(547, nd11122020.DeathConfirmed);
            Assert.AreEqual(34, nd11122020.DeathIncrease);
            Assert.AreEqual(6, nd11122020.DeathProbable);
            // TODO add a few more row examples
        }

        [Test]
        public async Task RequestData()
        {
            // Integration test checks that we can make an HTTP request
            var mock = new Mock<IHttpClientFactory>();
            mock.Setup((m) => m.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            var dataService = new CovidTrackingDataService(mock.Object);
            var stateData = await dataService.GetDailyStateRecordsAsync();
            var nationData = await dataService.GetDailyNationRecordsAsync();
            

            Assert.Greater((await stateData.ToArrayAsync()).Length, 0);
            Assert.Greater((await nationData.ToArrayAsync()).Length, 0);
        }
    }
}