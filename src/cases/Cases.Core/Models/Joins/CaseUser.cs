namespace Cases.Core.Models.Joins
{
    /// <summary>
    /// Join model two store the link between a given user and a case - the many to many relation - as
    /// a user can be linked ot many cases and a case can have many users linked to it
    /// 
    /// NOTE: we do not store any user info in this model as identity has user info we just store the user ID 
    /// </summary>
    public class CaseUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;
    }
}
