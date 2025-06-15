
using Ecom.API.Extentions;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Data.Identity;
using Ecom.infrastructure.Repository;
using Ecom.infrastructure.Repository.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Threading.Tasks;
using Talabat.APIS.Errors;
using Talabat.APIS.Middleware;

namespace Ecom.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddSwaggerServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // connection for SQL
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                  option.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });
            //Connection For Identity
            builder.Services.AddDbContext<AppIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            //Connection for Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            }
            );



            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<AppDbContext>(); //Context For Defult Connection For Sql
            var _Identitydbcontext = services.GetRequiredService<AppIdentityDbContext>(); //Context For identity Connection For Sql
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                 await _dbcontext.Database.MigrateAsync(); //Update DataBase
                 await _Identitydbcontext.Database.MigrateAsync(); //Update DataBase For Identity DB
                 await StoreContextSeeding.SeedAsync(_dbcontext); //Data Seeding
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during migration");
            }

    


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CORSPolicy");
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
