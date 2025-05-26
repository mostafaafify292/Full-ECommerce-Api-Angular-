using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env , IMemoryCache memoryCache)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext httpcontext)
        {
            if (IsRequiredAllow(httpcontext) == false )
            {
                httpcontext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                httpcontext.Response.ContentType = "application/json";
                var response = new ApiExceptionResponse((int)HttpStatusCode.TooManyRequests, "Too Many Requests ,Please Try Again Leter");
                await httpcontext.Response.WriteAsJsonAsync(response);
            }
            try
            {
                await _next.Invoke(httpcontext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                httpcontext.Response.ContentType = "application/json"; 
                httpcontext.Response.StatusCode =(int) HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment() ? new ApiExceptionResponse
                    ((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var option = new JsonSerializerOptions()
                { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, option);
                await httpcontext.Response.WriteAsync(json );
                
               
            }
        } 
        public bool IsRequiredAllow(HttpContext context) // Thats for Rate Limit Security 
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cashKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;

            var (TimesTemp, count) = _memoryCache.GetOrCreate(cashKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (TimesTemp: dateNow, count: 0);
            });
            if (dateNow-TimesTemp < _rateLimitWindow)
            {
                if (count >= 20 )
                {
                    return false;
                    
                }
                _memoryCache.Set(cashKey, (TimesTemp, count += 1), _rateLimitWindow);
            }
            else
            {
               // _memoryCache.Set(cashKey, (TimesTemp, count ), _rateLimitWindow);
                _memoryCache.Set(cashKey, (dateNow, 1), _rateLimitWindow);
            }
            return true;
        }
    }
}
