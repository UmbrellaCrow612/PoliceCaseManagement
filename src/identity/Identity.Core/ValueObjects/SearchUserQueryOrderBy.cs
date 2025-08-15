namespace Identity.Core.ValueObjects
{
    /// <summary>
    /// Extension for <see cref="SearchUserQuery"/> used for order of users by certain fields for search results
    /// </summary>
    public enum SearchUserQueryOrderBy
    {
        /// <summary>
        /// Order by the ID of the user's
        /// </summary>
        Id = 0,

        /// <summary>
        /// Order by user's username in ascending order
        /// </summary>
        UserNameAscending = 1,

        /// <summary>
        /// Order by user's username descending
        /// </summary>
        UserNameDescending = 2,

        /// <summary>
        /// Order by user's email ascending
        /// </summary>
        UserEmailAscending = 3,

        /// <summary>
        /// Order by user's email descending
        /// </summary>
        UserEmailDescending = 4,

        /// <summary>
        /// Order by user's phone number ascending
        /// </summary>
        UserPhoneNumberAscending = 5,

        /// <summary>
        /// Order by user's phone number descending
        /// </summary>
        UserPhoneNumberDescending = 6,
    }
}
