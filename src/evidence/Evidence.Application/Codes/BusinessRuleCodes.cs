namespace Evidence.Application.Codes
{
    /// <summary>
    /// Define all business rules which are error or expectations in the business logic that require us to comminate said error in a machine and human readable format
    /// </summary>
    public static class BusinessRuleCodes
    {
        /// <summary>
        /// Use when a <see cref="Core.Models.Tag"/> being created in the system has it's <see cref="Core.Models.Tag.Name"/> field taken i.e is used by another tag in the system, tag names are unique. 
        /// </summary>
        public static readonly string TAG_NAME_USED = "TAG_NAME_TAKEN";
    }
}
