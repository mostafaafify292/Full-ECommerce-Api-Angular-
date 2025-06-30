using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Repository.Services;
using Ecom.infrastructure.Repository;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Ecom.Core.ServicesContract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ecom.API.Extentions
{
    public static class AppServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IImageMangementService, ImageMangementService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerBasketRepository, CustomerBasketRepository>();
            services.AddMemoryCache();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddScoped<IAuth, AuthRepository>();
            services.AddScoped<IOrderService, OrderService>();


            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            );

            // Custom model validation response
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(p => p.Value.Errors.Count > 0)
                        .SelectMany(p => p.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            // JWT Validation
            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; //Angular is Https not http 
                options.SaveToken = true;

                var validIssuer = configuration["JWT:ValidIssuer"];
                var validAudience = configuration["JWT:ValidAudience"];
                var authKey = configuration["JWT:AuthKey"];

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = validIssuer,
                    ValidateAudience = true,
                    ValidAudience = validAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var tokenFromCookie = context.Request.Cookies["token"];                   

                        if (string.IsNullOrEmpty(tokenFromCookie))
                        {
                            tokenFromCookie = context.Request.Headers["Authorization"]
                                .ToString().Replace("Bearer ", "");
                        }

                        context.Token = tokenFromCookie;
                        return Task.CompletedTask;
                    }
                };

            });

            return services;
        }



        

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
