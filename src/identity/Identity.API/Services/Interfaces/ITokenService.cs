namespace Identity.API.Services.Interfaces
{
    /// <summary>
    /// Handles token realted actions
    /// </summary>
    public interface ITokenService
    {
        Task<TokenResult> SearchTokens(SearchTokenQuery query);

        Task<TokenResult> RevokeUserTokens(string userId);

        Task<TokenResult> GetUserTokens(string userId);
    }

    public class TokenResult
    {
        public bool IsSuccess { get; set; } = false;
        public ICollection<TokenResultError> Errors { get; private set; } = [];

        public void AddError(int code, string message) { Errors.Add(new TokenResultError { Code = code, Message = message}); }
    }

    public class TokenResultError
    {
        public required int Code { get; set; }
        public required string Message { get; set; }
    }

    public class SearchTokenQuery
    {

    }
}
