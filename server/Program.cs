using EventManager.AppServices.Implementations;
using EventManager.AppServices.Interfaces;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using EventManager.Data.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EventManager
{
    /// <summary>
    /// Главният клас за стартиране на EventManager приложението.
    /// Съдържа конфигурацията на услугите (DI), middleware конвейера
    /// и стартирането на уеб сървъра.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка за вход в приложението.
        /// Конфигурира DI контейнера, аутентикацията (JWT + Identity),
        /// Swagger документацията, CORS политиката, базата данни и
        /// стартира уеб сървъра.
        /// </summary>
        /// <param name="args">
        /// Аргументи от командния ред, които ASP.NET Core може да използва
        /// при стартиране на приложението.
        /// </param>
        /// <returns>
        /// Асинхронна задача, която приключва, когато приложението бъде спряно.
        /// </returns>
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<EventManagerDbContext>(x =>
                x.UseSqlServer(connectionString));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = false; // няма смисъл да пазим в контекст
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.FromSeconds(30) // по-строг гратис
                    };

                    // сигнализирай фронтенда за изтекъл токен
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            if (ctx.Exception is SecurityTokenExpiredException)
                                ctx.Response.Headers["Token-Expired"] = "true";
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services
                .AddIdentityCore<User>(opt =>
                {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 4;
                    opt.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<EventManagerDbContext>()
                .AddDefaultTokenProviders();

            builder.Services
            .AddControllers()
            // to prevent circular reference issues
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
             });
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EventManager API",
                    Description = "An ASP.NET Core Web API for managing events.",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://example.com/license")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, new string[] {}
                    }
                });
            });

            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<IInvitesService, InvitesService>();
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();

            builder.Services.AddHttpContextAccessor();

            var ClientAppPolicy = "ClientAppPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: ClientAppPolicy,
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:5173") // TODO: extract frontend URL to config file or environment variable
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                      });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(ClientAppPolicy);
            app.UseHttpsRedirection();



            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                await RoleDataSeeder.SeedRolesAsync(roleManager);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var adminEmail = "admin@eventmanager.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var user = new User
                    {
                        UserName = "admin",
                        Email = adminEmail
                    };

                    // !Важно: сложи силна парола за демонстрацията
                    var result = await userManager.CreateAsync(user, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }

            await app.RunAsync();
        }
    }
}
