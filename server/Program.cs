using EventManager.AppServices.Implementations;
using EventManager.AppServices.Interfaces;

namespace EventManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
