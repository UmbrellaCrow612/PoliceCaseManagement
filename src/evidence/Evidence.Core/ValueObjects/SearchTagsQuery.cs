namespace Evidence.Core.ValueObjects
{
    /// <summary>
    /// Object to search <see cref="Models.Tag"/> in the system
    /// </summary>
    public class SearchTagsQuery
    {
        /// <summary>
        /// The name of the tag
        /// </summary>
        public required string? Name { get; set; } = null;

        /// <summary>
        /// How many items you want per page
        /// </summary>
        public required int? PageSize { get; set; } = null;

        /// <summary>
        /// The page number you want to get
        /// </summary>
        public required int? PageNumber { get; set; } = null;
    }
}
