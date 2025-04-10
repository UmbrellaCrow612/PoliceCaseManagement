namespace Cases.Application.Codes
{
    /// <summary>
    /// Contains all BusinessRuleCodes for the cases application
    /// </summary>
    public class BusinessRuleCodes
    {
        /// <summary>
        /// Indicates that a <see cref="Cases.Core.Models.IncidentType"/> already exists and you are trying to create a new 
        /// one that has the same name.
        /// </summary>
        public const string IncidentTypeAlreadyExists = "INCIDENT_TYPE_ALREADY_EXISTS";
    }
}
