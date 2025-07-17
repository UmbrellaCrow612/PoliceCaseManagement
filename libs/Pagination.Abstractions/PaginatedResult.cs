namespace Pagination.Abstractions
{
    /// <summary>
    /// Represents a paginated result containing a collection of items,
    /// pagination metadata, and navigation links.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// The collection of data items for the current page.
        /// </summary>
        public required IEnumerable<T> Data { get; set; } = [];

        /// <summary>
        /// Metadata describing the pagination state.
        /// </summary>
        public required PaginationMetadata Pagination { get; set; }

        /// <summary>
        /// If there is a next page that can be fetched
        /// </summary>
        public required bool HasNextPage { get; set; }

        /// <summary>
        /// If there is a previous page to fetch
        /// </summary>
        public required bool HasPreviousPage { get; set; }
    }
}
