namespace Utils.DTOs
{
    /// <summary>
    /// Represents a specific error detail with field and reason
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// Name of the field causing the validation error
        /// </summary>
        public required string Field { get; set; }

        /// <summary>
        /// Reason for the validation error
        /// </summary>
        public required string Reason { get; set; }
    }
}
