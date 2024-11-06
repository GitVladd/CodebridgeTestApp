
using CodebridgeTestApp.DataDbContext;
using CodebridgeTestApp.Middleware;
using CodebridgeTestApp.Repository;
using CodebridgeTestApp.Service;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace CodebridgeTestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IDogRepository, DogRepository>();
            services.AddScoped<IDogService, DogService>();


            var tokenLimit = configuration.GetSection("RateLimiter").GetValue<int>("TokenLimit");

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = 429;
                options.AddFixedWindowLimiter(policyName: "DefaultPolicy", options =>
                {
                    options.PermitLimit = tokenLimit;
                    options.Window = TimeSpan.FromSeconds(1);
                    options.AutoReplenishment = true;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 0;
                });
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        private static void Configure(WebApplication app)
        {
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers().RequireRateLimiting("DefaultPolicy");
        }
    }
}
