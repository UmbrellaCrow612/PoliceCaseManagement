namespace Results.Abstractions
{
    /// <summary>
    /// Represents the standard format for <see cref="IResult"/> errors.
    /// </summary>
    public interface IResultError
    {
        /// <summary>
        /// A standardized string code representing the error that occurred.
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// An optional message providing additional information about the error.
        /// </summary>
        string? Message { get; set; }
    }
}
