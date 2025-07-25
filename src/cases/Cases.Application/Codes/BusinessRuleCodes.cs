﻿namespace Cases.Application.Codes
{
    /// <summary>
    /// Contains all BusinessRuleCodes for the cases application
    /// </summary>
    public static class BusinessRuleCodes
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

        /// <summary>
        /// Indicates that a validation error occurred, could be more general than a known business rule error - typically used when a field is missing or 
        /// miss configured in a model
        /// </summary>
        public const string ValidationError = "VALIDATION_ERROR";

        /// <summary>
        /// Indicates that the user is already linked to a case
        /// </summary>
        public const string UserAlreadyAssignedToCase = "USER_ALREADY_LINKED_TO_CASE";

        /// <summary>
        /// Indicates that a user dose not exist
        /// </summary>
        public const string UserNotFound = "USER_NOT_FOUND";

        /// <summary>
        /// Indicates that a piece of evidence was not found
        /// </summary>
        public const string EvidenceNotFound = "EVIDENCE_NOT_FOUND";

        /// <summary>
        /// Indicates that a piece of evidence is already linked to a case
        /// </summary>
        public const string EvidenceAlreadyLinked = "EVIDENCE_ALREADY_LINKED";

        /// <summary>
        /// Indicates that a piece of evidence is not linked to a case
        /// </summary>
        public const string EvidenceNotLinked = "EVIDENCE_NOT_LINKED";
    }
}
