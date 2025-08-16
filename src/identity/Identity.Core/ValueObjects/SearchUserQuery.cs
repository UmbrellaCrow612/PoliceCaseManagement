namespace Identity.Core.ValueObjects
{
    /// <summary>
    /// Value object for how to search for users based on on there properties
    /// </summary>
    public class SearchUserQuery
    {
        /// <summary>
        /// Search by a user's username
        /// </summary>
        public string? UserName { get; set; } = null;

        /// <summary>
        /// Search by a user's email
        /// </summary>
        public string? Email { get; set; } = null;

        /// <summary>
        /// Search by a user's phone number 
        /// </summary>
        public string? PhoneNumber { get; set; } = null;

        /// <summary>
        /// By default it will be set to <see cref="SearchUserQueryOrderBy.Id"/> unless provided by client
        /// </summary>
        public SearchUserQueryOrderBy OrderBy { get; set; } = SearchUserQueryOrderBy.Id;

        /// <summary>
        /// The page number you want to fetch
        /// </summary>
        public required int PageNumber { get; set; }

        /// <summary>
        /// The page size you want to fetch
        /// </summary>
        public required int PageSize { get; set; }
    }
}
