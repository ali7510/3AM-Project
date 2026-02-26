
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Persistence.Data.DataSeed;
using Ecommerce.Persistence.Data.DBcontexts;
using Ecommerce.Persistence.Repository;
using Ecommerce.Service;
using Ecommerce.Service.AuthServices;
using Ecommerce.Service.DashboardServices;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.Service.PaymentServices;
using Ecommerce.Service.ProductServices;
using Ecommerce.Service.ProfileServices;
using Ecommerce.ServiceAbstraction;
using Ecommerce.ServiceAbstraction.AuthServices;
using Ecommerce.ServiceAbstraction.IAuthServices;
using Ecommerce.ServiceAbstraction.IDashboardServices;
using Ecommerce.ServiceAbstraction.IPaymentServices;
using Ecommerce.ServiceAbstraction.IProductServices;
using Ecommerce.ServiceAbstraction.IProfileServices;
using ECommerce_3AM.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            //builder.Services.AddSwaggerGen();
            // ========================================================================
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                            });
            builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<StoreIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(x=>x.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(x=>x.AddProfile<CartProfile>());
            builder.Services.AddAutoMapper(x=>x.AddProfile<AuthProfile>());
            builder.Services.AddAutoMapper(x=>x.AddProfile<OrderProfile>());
            builder.Services.AddAutoMapper(x=>x.AddProfile<AccountProfile>());
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddHttpClient("MyFatoorah", client =>
            {
                var baseUrl = builder.Configuration["MyFatoorah:BaseUrl"]!;
                var apiKey = builder.Configuration["MyFatoorah:ApiKey"]!;

                Console.WriteLine($"BaseUrl: {baseUrl}");
                Console.WriteLine($"ApiKey: {apiKey}");

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            });
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.Configure<MyFatoorahSettings>(
                builder.Configuration.GetSection("MyFatoorah"));
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                        )
                    };
                });

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

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

            app.MapControllers();

            app.Run();
        }
    }
}
