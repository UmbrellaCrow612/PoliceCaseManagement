namespace Validator
{
    /// <summary>
    /// Result object returned <see cref="Validator{T}"/>
    /// </summary>
    /// <typeparam name="T">The class that <see cref="Validator{T}"/> was written for.</typeparam>
    public class ValidatorResult<T>
    {
        /// <summary>
        /// Flag to indicate if all validators rules defined for <see cref="T"/> passed.
        /// </summary>
        public bool IsSuccessful { get; set; } = false;

        /// <summary>
        /// A List of error messages for <see cref="Validator{T}"/> for class <see cref="T"/>
        /// </summary>
        public List<string> Errors { get; set; } = [];
    }
}
