namespace SoftDelete.Abstractions
{
    /// <summary>
    /// Contract used for items that support soft delete - soft delete is when you do not want to remove the item from your storage 
    /// but want fields to filter those items out when querying
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// DateTime when the item was deleted - Also used as the flag to filter items out, if it has a value it means it is deleted as well as when (UTC time)
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Optional; ID of the person who deleted the item
        /// </summary>
        public string? DeletedById { get; set; }

        /// <summary>
        /// Mark the item as deleted
        /// </summary>
        /// <param name="deletedById">Optional; ID of the person performing the deletion</param>
        public void Delete(string? deletedById = null);

        /// <summary>
        /// Helper method that returns if a the given item is deleted; <see cref="DeletedAt"/> and <see cref="DeletedById"/> have a value
        /// </summary>
        public bool IsDeleted();
    }
}
