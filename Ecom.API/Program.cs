
using Ecom.API.Extentions;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repository;
using Ecom.infrastructure.Repository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
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

            builder.Services.AddCors(op =>
            op.AddPolicy("CORSPolicy", builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200");
            })
            );


            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });



            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<AppDbContext>();
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                 await _dbcontext.Database.MigrateAsync(); //Update DataBase
                 await StoreContextSeeding.SeedAsync(_dbcontext); //Data Seeding
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during migration");
            }
            app.UseCors("CORSPolicy");
            app.UseMiddleware<ExceptionMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseStaticFiles();
            app.Run();
        }
    }
}
