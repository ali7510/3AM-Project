
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Persistence.Data.DataSeed;
using Ecommerce.Persistence.Data.DBcontexts;
using Ecommerce.Persistence.IdentityUser.DBcontext;
using Ecommerce.Persistence.Repository;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.Service.ProductServices;
using Ecommerce.ServiceAbstraction.IProductServices;
using ECommerce_3AM.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_3AM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<StoreIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(x=>x.AddProfile<ProductProfile>());
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            #region DataSeed
            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
