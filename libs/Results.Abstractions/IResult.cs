namespace Results.Abstractions
{
    /// <summary>
    /// Represents a standard result object for operations.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        bool Succeeded { get; set; }

        /// <summary>
        /// A collection of errors that occurred during the operation.
        /// </summary>
        ICollection<IResultError> Errors { get; set; }

        /// <summary>
        /// Adds an error to the result object.
        /// </summary>
        /// <param name="code">A standardized string code representing the error.</param>
        /// <param name="message">An optional message describing the error.</param>
        void AddError(string code, string? message = null);
    }
}
