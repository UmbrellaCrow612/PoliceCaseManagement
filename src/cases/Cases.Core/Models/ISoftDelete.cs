namespace Cases.Core.Models
{
    /// <summary>
    /// Used to add soft deletion to a model meaning it dose not get fully removed from a database 
    /// but a flag is added to remove it from query's, there should be a global query filter on such model meaning deleted items 
    /// are filtered out on every query with a global query
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Flag to indicate that the item is considered deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Date time when the item was deleted in UTC time
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
