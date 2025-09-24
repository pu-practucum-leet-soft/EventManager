using EventManager.AppServices.Implementations;
using EventManager.AppServices.Interfaces;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using EventManager.Data.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManager
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<EventManagerDbContext>(x =>
                x.UseSqlServer(connectionString));

            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<EventManagerDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllers();

            builder.Services.AddScoped<IEventService, EventService>();

            builder.Services.AddEndpointsApiExplorer();

            var ClientAppPolicy = "ClientAppPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: ClientAppPolicy,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:5173") // TODO: extract frontend URL to config file or environment variable
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                      });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();


            app.UseCors(ClientAppPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleDataSeeder.SeedRolesAsync(roleManager);
            }

            await app.RunAsync();
        }
    }
}
