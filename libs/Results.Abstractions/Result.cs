
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

        public override string ToString()
        {
            if (Errors == null || Errors.Count == 0)
                return "No errors.";

            return string.Join("; ", Errors.Select(e =>
                string.IsNullOrEmpty(e.Message) ? e.Code : $"{e.Code}: {e.Message}"));
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
