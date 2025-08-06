namespace Cases.Core.ValueObjects
{
    /// <summary>
    /// Represents the role a person has on a case.
    /// </summary>
    public enum CasePersonRole
    {
        /// <summary>
        /// A person suspected of involvement in the case.
        /// </summary>
        Suspect = 0,

        /// <summary>
        /// A person of interest, but not formally a suspect.
        /// </summary>
        PersonOfInterest = 1,

        /// <summary>
        /// A person who witnessed the events related to the case.
        /// </summary>
        Witness = 2,

        /// <summary>
        /// The person who reported the incident.
        /// </summary>
        Complainant = 3,

        /// <summary>
        /// A person harmed or impacted by the incident.
        /// </summary>
        Victim = 4
    }
}
