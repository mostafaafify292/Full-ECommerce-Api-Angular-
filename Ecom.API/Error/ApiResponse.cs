namespace Ecom.API.Error
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultValueForStatusCode(StatusCode);
        }

        private string? GetDefaultValueForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                404 => "Not Found",
                400 => "Bad Request",
                401 => "Un Authorize",
                500 => "Server Error",
                _ => null
            };
        }
    }
}
