namespace PoliceCaseManagement.Application.DTOs.Errors
{
    public class ErrorResponseDto
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }

        public ErrorResponseDto(string message, string details = null)
        {
            Message = message;
            Details = details;
            Timestamp = DateTime.UtcNow;
        }
    }
}
