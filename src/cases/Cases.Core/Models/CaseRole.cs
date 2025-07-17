namespace Cases.Core.Models
{
    /// <summary>
    /// Role a user has in a given case.
    /// </summary>
    public enum CaseRole
    {
        /// <summary>
        /// The role of the person who created the <see cref="Case"/>. Has the highest privileges.
        /// </summary>
        Owner = 0,

        /// <summary>
        /// The role of a person who can view and edit the <see cref="Case"/> they are assigned to.
        /// </summary>
        Editor = 1,

        /// <summary>
        /// The role of a person who can only view the <see cref="Case"/> they are assigned to.
        /// </summary>
        Viewer = 2,
    }
}
