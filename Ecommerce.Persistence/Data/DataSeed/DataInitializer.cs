using Ecommerce.Domain;
using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Domain.UserModule;
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
                var hasUsers = await _dbContext.Users.AnyAsync();
                var hasOrders = await _dbContext.Orders.AnyAsync();
                var hasOrderItems = await _dbContext.OrderItems.AnyAsync();
                var hasCarts = await _dbContext.Carts.AnyAsync();
                var hasCartItems = await _dbContext.CartItems.AnyAsync();
                var hasPayments = await _dbContext.Payments.AnyAsync();

                Console.WriteLine($"Has Products: {hasProducts}, Has Categories: {hasCategories}");

                if (!hasCategories)
                {
                    Console.WriteLine("Seeding categories...");
                    await SeedDataFromJson<Category>("Categories.json", _dbContext.Categories);
                }

                if (!hasUsers)
                {
                    Console.WriteLine("Seeding users...");
                    await SeedDataFromJson<User>("Users.json", _dbContext.Users);
                }

                if (!hasProducts)
                {
                    Console.WriteLine("Seeding products...");
                    await SeedDataFromJson<Product>("Products.json", _dbContext.Products);
                }

                if (!hasCarts)
                {
                    Console.WriteLine("Seeding carts...");
                    await SeedDataFromJson<Cart>("Carts.json", _dbContext.Carts);
                }


                if (!hasOrders)
                {
                    Console.WriteLine("Seeding orders...");
                    await SeedDataFromJson<Order>("Orders.json", _dbContext.Orders);
                }


                if (!hasCartItems)
                {
                    Console.WriteLine("Seeding cart items...");
                    await SeedDataFromJson<CartItem>("CartItems.json", _dbContext.CartItems);
                }

                if (!hasOrderItems)
                {
                    Console.WriteLine("Seeding order items...");
                    await SeedDataFromJson<OrderItem>("OrderItems.json", _dbContext.OrderItems);
                }

                if (!hasPayments)
                {
                    Console.WriteLine("Seeding payments...");
                    await SeedDataFromJson<Payment>("Payments.json", _dbContext.Payments);
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
                Console.WriteLine("MAIN ERROR: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("INNER ERROR: " + ex.InnerException.Message);
                }

                throw;
            }
        }
    }
}
