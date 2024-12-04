namespace Shared.DTOs
{
    /// <summary>
    /// Represents a standardized API error response structure
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Default status of the error response
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// HTTP status code representing the error
        /// </summary>
        public required int StatusCode { get; set; }

        /// <summary>
        /// High-level error message describing the issue
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Detailed list of specific errors
        /// </summary>
        public List<ErrorDetail> Errors { get; set; } = [];
    }
}
