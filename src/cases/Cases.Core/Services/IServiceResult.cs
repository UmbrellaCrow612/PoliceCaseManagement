namespace Cases.Core.Services
{
    /// <summary>
    /// Standard result object for a cases service
    /// </summary>
    public interface IServiceResult
    {
        /// <summary>
        /// If the operation was successful 
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// List of error that could have occurred
        /// </summary>
        public ICollection<IServiceError> Errors { get; set; }

        /// <summary>
        /// Helper method to add errors to the result object
        /// </summary>
        /// <param name="code">A string standardized code representing the error that has occurred </param>
        /// <param name="message">A optional message</param>
        public void AddError(string code, string? message);

    }
}
