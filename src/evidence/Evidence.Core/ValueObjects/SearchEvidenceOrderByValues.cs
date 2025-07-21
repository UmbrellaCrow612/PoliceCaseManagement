namespace Evidence.Core.ValueObjects
{
    /// <summary>
    /// A enum representing the ways in which one can order <see cref="Models.Evidence"/> when searching them
    /// using <see cref="SearchEvidenceQuery"/>
    /// </summary>
    public enum SearchEvidenceOrderByValues
    {
        /// <summary>
        /// Will order items based on <see cref="Models.Evidence.CollectionDate"/> field
        /// </summary>
        CollectionDate = 0,

        /// <summary>
        /// Will order items based on <see cref="Models.Evidence.UploadedAt"/>
        /// </summary>
        UploadedAt = 1,
    }
}
