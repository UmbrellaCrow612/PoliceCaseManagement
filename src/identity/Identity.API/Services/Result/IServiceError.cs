namespace Identity.API.Services.Result
{
    /// <summary>
    /// Represents a base error object used by all services in this API.
    /// This serves as the standard error format and can be extended if necessary.
    /// </summary>
    public interface IServiceError
    {
        /// <summary>
        /// The error code. Use <see cref="Result.Codes"/> for standard project codes.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Additional information about the error, if available.
        /// </summary>
        public string? Message { get; set; }
    }
}
