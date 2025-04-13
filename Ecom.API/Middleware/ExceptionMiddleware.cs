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

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext httpcontext)
        {
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
    }
}
