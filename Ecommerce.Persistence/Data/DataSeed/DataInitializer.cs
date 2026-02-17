using Ecommerce.Domain;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Persistence.Data.DBcontexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                // Force apply migrations first
                await _dbContext.Database.MigrateAsync();

                var hasProducts = await _dbContext.Products.AnyAsync();
                var hasCategories = await _dbContext.Categories.AnyAsync();

                Console.WriteLine($"Has Products: {hasProducts}, Has Categories: {hasCategories}");

                if (hasCategories && hasProducts) return;

                if (!hasCategories)
                {
                    Console.WriteLine("Seeding categories...");
                    await SeedDataFromJson<Category>("Categories.json", _dbContext.Categories);
                }

                if (!hasProducts)
                {
                    Console.WriteLine("Seeding products...");
                    await SeedDataFromJson<Product>("Products.json", _dbContext.Products);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"INIT ERROR: {ex.Message}");
                Console.WriteLine($"STACK: {ex.StackTrace}");
            }
        }

        private async Task SeedDataFromJson<T>(string filename, DbSet<T> dbSet) where T : BaseEntity
        {
            var assemblyLocation = Path.GetDirectoryName(typeof(DataInitializer).Assembly.Location)!;
            var filePath = Path.Combine(assemblyLocation,"Data//DataSeed", "JSONFiles", filename);
            Console.WriteLine($"Looking for file at: {filePath}");
            Console.WriteLine($"File exists: {File.Exists(filePath)}");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filename} was not found at path: {filePath}");
            }

            try
            {
                using var dataStream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data != null && data.Count > 0)
                {
                    await dbSet.AddRangeAsync(data);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error seeding data from {filename}: {ex.Message}");
                throw;
            }


        }
    }
}
