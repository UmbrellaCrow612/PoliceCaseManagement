namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between an <see cref="Entities.Arrest"/> and <see cref="Entities.User"/>
    /// </summary>
    public class ArrestUser
    {
        public required string ArrestId { get; set; }
        public required string UserId { get; set; }


        public User? User { get; set; } = null;
        public Arrest? Arrest { get; set; } = null;
    }
}
