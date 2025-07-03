namespace Pagination.Abstractions
{
    /// <summary>
    /// Contains metadata information about the pagination state of a result set.
    /// </summary>
    public class PaginationMetadata
    {
        /// <summary>
        /// The current page number (1-based).
        /// </summary>
        public required int CurrentPage { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public required int PageSize { get; set; }

        /// <summary>
        /// The total number of pages available.
        /// </summary>
        public required int TotalPages { get; set; }

        /// <summary>
        /// The total number of records across all pages.
        /// </summary>
        public required int TotalRecords { get; set; }
    }
}
