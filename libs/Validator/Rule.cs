namespace Validator
{
    /// <summary>
    /// A rule to run for a <see cref="Validator.Validator{T}"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Validator{T}"/> type passed down.</typeparam>
    public class Rule<T>
    {
        /// <summary>
        /// An error message to show if the <see cref="Func"/> returns false and it did not pass the check
        /// </summary>
        public required string? Message { get; set; }

        /// <summary>
        /// A expression that is run against <see cref="T"/> that validates a field and retuns a boolean
        /// </summary>
        public required Func<T, bool> Func { get; set; }
    }
}
