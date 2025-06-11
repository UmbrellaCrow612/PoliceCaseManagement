namespace Cases.Core.Services
{
    /// <summary>
    /// The standard service error for cases services
    /// </summary>
    public interface IServiceError
    {
        /// <summary>
        /// A string standardized code representing the error that has occurred 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// A optional string message with some extra information
        /// </summary>
        public string? Message { get; set; }
    }
}
