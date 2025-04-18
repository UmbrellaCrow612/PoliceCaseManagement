﻿namespace Cases.Application.Codes
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

        /// <summary>
        /// Indicates that a case number is taken when you are trying to create a <see cref="Case"/> as they should be unique.
        /// </summary>
        public const string CaseNumberTaken = "CASE_NUMBER_TAKEN";

        /// <summary>
        /// Indicates that a IncidentType is already linked to a case.
        /// </summary>
        public const string IncidentTypeAlreadyLinkedToCase = "INCIDENT_TYPE_ALREADY_LINKED_TO_CASE";

    }
}
