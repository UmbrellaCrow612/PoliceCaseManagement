
namespace Results.Abstractions
{
    /// <inheritdoc />
    public class Result : IResult
    {
        /// <inheritdoc />
        public bool Succeeded { get; set; } = false;

        /// <inheritdoc />
        public List<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new ResultError { Code = code, Message = message });
        }
    }

    /// <inheritdoc />
    public class ResultError : IResultError
    {
        /// <inheritdoc />
        public required string Code { get; set; }

        /// <inheritdoc />
        public required string? Message { get; set; }
    }
}
