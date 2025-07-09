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

        /// <summary>
        /// Use when a user ID dose not exist
        /// </summary>
        public static readonly string USER_DOES_NOT_EXIST = "USER_DOES_NOT_EXIST";

        /// <summary>
        /// UsE when a <see cref="Core.Models.Evidence"/> being created and it's ref number is already taken by another as they are unique
        /// </summary>
        public static readonly string EVIDENCE_REFERENCE_NUMBER_TAKEN = "EVIDENCE_REFERENCE_NUMBER_TAKEN";

        /// <summary>
        /// Use this when updating a evidence item and it's ref number has been changed - this is not allowed
        /// </summary>
        public static readonly string EVIDENCE_REFERENCE_CHANGED = "EVIDENCE_REFERENCE_CHANGED";
    }
}
