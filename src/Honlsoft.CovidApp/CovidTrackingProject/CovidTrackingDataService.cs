using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Honlsoft.CovidApp.Data;

namespace Honlsoft.CovidApp.CovidTrackingProject
{
    public class CovidTrackingDataService : ICovidTrackingDataService
    {
        private readonly IHttpClientFactory _clientFactory;
        
        private class MyInt32Converter : Int32Converter {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return (int)0;
                }
                
                return base.ConvertFromString(text, row, memberMapData);
            }
        }
        
        public CovidTrackingDataService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IAsyncEnumerable<CovidStateDailyRecord>> GetDailyStateRecordsAsync()
        {
            var httpClient = _clientFactory.CreateClient();
            var stream = await httpClient.GetStreamAsync("https://covidtracking.com/data/download/all-states-history.csv");
            return ParseCsvRecords<CovidStateDailyRecord>(new StreamReader(stream));
        }

        public async Task<IAsyncEnumerable<CovidNationDailyRecord>> GetDailyNationRecordsAsync()
        {
            var httpClient = _clientFactory.CreateClient();
            var stream = await httpClient.GetStreamAsync("https://covidtracking.com/data/download/national-history.csv");
            return ParseCsvRecords<CovidNationDailyRecord>(new StreamReader(stream));
        }

        public static async IAsyncEnumerable<TRecordType> ParseCsvRecords<TRecordType>(TextReader textReader)
        {
            
            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                PrepareHeaderForMatch = (header, idx) => header.ToLower(),
            };
            config.TypeConverterCache.RemoveConverter<Int32Converter>();
            config.TypeConverterCache.AddConverter<int>(new MyInt32Converter());

            CsvReader reader = new CsvReader(textReader, config);
            
            if (await reader.ReadAsync())
            {
                reader.ReadHeader();
                while (await reader.ReadAsync())
                {
                    var record = reader.GetRecord<TRecordType>();
                    if (record is ISourceHash sourceRecord)
                    {
                        var hash = CalculateHash(reader.Context.Record);
                        sourceRecord.SourceHash = hash;
                    }
                    yield return record;
                }
            }
        }

        private static byte[] CalculateHash(string[] records)
        {
            using SHA256Managed shaHash = new();
            var allRecords = string.Join("", records);
            var bytes = Encoding.UTF8.GetBytes(allRecords);
            return shaHash.ComputeHash(bytes);
        }
    }
}