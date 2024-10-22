namespace PoliceCaseManagement.Api.Exceptions
{
    public class ApiException(string message, int statusCode = 500) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}
