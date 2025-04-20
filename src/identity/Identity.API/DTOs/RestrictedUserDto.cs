namespace Identity.API.DTOs
{
    /// <summary>
    /// Used to send user details but less than a <see cref="UserDto"/>
    /// </summary>
    public class RestrictedUserDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
    }
}
