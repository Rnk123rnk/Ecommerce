using System.Net;

namespace Ecommerce.Models.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessage = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string>? ErrorMessage { get; set; }
        public string? ExceptionMessage { get; set; }
        public object? Result { get; set; }
    }
}
