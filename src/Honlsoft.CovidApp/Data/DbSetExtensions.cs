
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Honlsoft.CovidApp.Data
{
    public static class DbSetExtensions
    {
        public static TEntity LoadDataSeed<TEntity>(string fileName)
        {
            var data = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), "Data", fileName));
            var entity = JsonSerializer.Deserialize<TEntity>(data);
            return entity;
        }

        public static void ImportData<TEntity>(this DbSet<TEntity> dbSet, string fileName) where TEntity: class
        {
            var data = LoadDataSeed<TEntity[]>(fileName);
            dbSet.AddRange(data);
        }
    }
}