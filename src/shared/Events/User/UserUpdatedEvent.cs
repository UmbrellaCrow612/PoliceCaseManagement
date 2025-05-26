namespace Events.User
{
    /// <summary>
    /// Event fired off when a user details have changed
    /// </summary>
    public class UserUpdatedEvent
    {
        /// <summary>
        /// The ID of the user being updated
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// The UserName of the user
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The Email of the user
        /// </summary>
        public required string Email { get; set; }
    }
}
