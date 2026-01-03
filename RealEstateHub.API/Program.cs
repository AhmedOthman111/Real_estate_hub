
using Hangfire;
using RealEstateHub.API.Middlewares;
using RealEstateHub.Infrastructure;
using RealEstateHub.Infrastructure.DataSeeder;

namespace RealEstateHub.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddInfrastructure(builder.Configuration);





            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseHangfireDashboard("/hangfire");
           
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IdentitySeeder.SeedAsync(services);
            }


            app.Run();
        }
    }
}
