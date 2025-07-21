namespace Evidence.Core.ValueObjects
{
    /// <summary>
    /// Way to search <see cref="Models.Evidence"/>
    /// </summary>
    public class SearchEvidenceQuery
    {
        /// <summary>
        /// Refers to <see cref="Models.Evidence.ReferenceNumber"/>
        /// </summary>
        public string? ReferenceNumber { get; set; } = null;

        /// <summary>
        /// Refers to <see cref="Models.Evidence.FileName"/>
        /// </summary>
        public string? FileName { get; set; } = null;

        /// <summary>
        /// Refers to <see cref="Models.Evidence.ContentType"/>
        /// </summary>
        public string? ContentType { get; set; } = null;

        /// <summary>
        /// Refers to <see cref="Models.Evidence.UploadedAt"/>
        /// </summary>
        public DateTime? UploadedAt { get; set; } = null;

        /// <summary>
        /// Refers to <see cref="Models.Evidence.CollectionDate"/>
        /// </summary>
        public DateTime? CollectionDate { get; set; } = null;

        /// <summary>
        /// The page number they want
        /// </summary>
        public required int PageNumber { get; set; }

        /// <summary>
        /// How many items they want per page
        /// </summary>
        public int? PageSize { get; set; } = null;

        /// <summary>
        /// Order by <see cref="SearchEvidenceOrderByValues"/>
        /// </summary>
        public SearchEvidenceOrderByValues? OrderBy { get; set; } = null; 

        /// <summary>
        /// The ID of the user who uploaded the piece of evidence <see cref="Models.Evidence.UploadedById"/>
        /// </summary>
        public string? UploadedById { get; set; } = null;
    }
}
