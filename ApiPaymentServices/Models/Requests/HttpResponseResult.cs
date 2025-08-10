using System.Net;

namespace ApiPaymentServices.Models.Requests
{
    public class HttpResponseResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static HttpResponseResult<T> Ok(T data, string? message = null)
        {
            return new HttpResponseResult<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static HttpResponseResult<T> Created(T data, string? message = null)
        {
            return new HttpResponseResult<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.Created
            };
        }

        public static HttpResponseResult<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new HttpResponseResult<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}
