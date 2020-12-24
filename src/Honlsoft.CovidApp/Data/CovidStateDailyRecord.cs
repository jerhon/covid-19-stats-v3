using System;

namespace Honlsoft.CovidApp.Data
{
    public class CovidStateDailyRecord : ISourceHash
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; }
        public string DataQualityGrade { get; set; }
        public int? Death { get; set; }
        public int? DeathConfirmed { get; set; }
        public int? DeathIncrease { get; set; }
        public int? DeathProbable { get; set; }
        public int? Hospitalized { get; set; }
        public int? HospitalizedCumulative { get; set; }
        public int? HospitalizedCurrently { get; set; }
        public int? HospitalizedIncrease { get; set; }
        public int? InIcuCumulative { get; set; }
        public int? InIcuCurrently { get; set; }
        public int? Negative { get; set; }
        public int? NegativeIncrease { get; set; }
        public int? NegativeTestsAntibody { get; set; }
        public int? NegativeTestsPeopleAntibody { get; set; }
        public int? NegativeTestsViral { get; set; }
        public int? OnVentilatorCumulative { get; set; }
        public int? OnVentilatorCurrently { get; set; }
        public int? Positive { get; set; }
        public int? PositiveCasesViral { get; set; }
        public int? PositiveIncrease { get; set; }
        public int? PositiveScore { get; set; }
        public int? PositiveTestsAntibody { get; set; }
        public int? PositiveTestsAntigen { get; set; }
        public int? PositiveTestsPeopleAntibody { get; set; }
        public int? PositiveTestsPeopleAntigen { get; set; }
        public int? PositiveTestsViral { get; set; }
        public int? Recovered { get; set; }
        public int? TotalTestEncountersViral { get; set; }
        public int? TotalTestEncountersViralIncrease { get; set; }
        public int? TotalTestResults { get; set; }
        public int? TotalTestResultsIncrease { get; set; }
        public int? TotalTestsAntibody { get; set; }
        public int? TotalTestsAntigen { get; set; }
        public int? TotalTestsPeopleAntibody { get; set; }
        public int? TotalTestsPeopleAntigen { get; set; }
        public int? TotalTestsPeopleViral { get; set; }
        public int? TotalTestsPeopleViralIncrease { get; set; }
        public int? TotalTestsViral { get; set; }
        public int? TotalTestsViralIncrease { get; set; }
        
        /// <summary>
        /// Provides a hash for comparison as to whether rows have changed.
        /// </summary>
        public byte[] SourceHash { get; set; }
    }
}