namespace SoftDelete.Abstractions
{
    /// <summary>
    /// Contract used for items that support soft delete - soft delete is when you do not want to remove the item from your storage 
    /// but want fields to filter those items out when querying
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Flag to indicate if the item is considered to be deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// DateTime when the item was deleted
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// The ID of the person who deleted the item
        /// </summary>
        public string? DeletedById { get; set; }

        /// <summary>
        /// Mark the item as deleted
        /// </summary>
        /// <param name="deletedById">The ID of the person performing the deletion</param>
        public void Delete(string deletedById);
    }
}
