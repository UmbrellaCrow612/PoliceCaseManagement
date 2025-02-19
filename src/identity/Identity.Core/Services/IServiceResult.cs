namespace Identity.Core.Services
{
    /// <summary>
    /// Represents a base result object used by all services in this API.
    /// This standardizes the structure of service responses and can be extended as needed.
    /// </summary>
    public interface IServiceResult
    {
        /// <summary>
        /// Indicates whether the service operation was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// A collection of errors that may have occurred during the service operation.
        /// </summary>
        public ICollection<IServiceError> Errors { get; set; }
    }
}