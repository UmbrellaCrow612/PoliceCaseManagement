namespace Identity.Core.ValueObjects
{
    /// <summary>
    /// Value object for how to search for users based on querys
    /// </summary>
    public class SearchUserQuery
    {
        public string? UserName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
    }
}
