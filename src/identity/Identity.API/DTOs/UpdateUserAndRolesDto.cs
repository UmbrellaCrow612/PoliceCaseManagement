namespace Identity.API.DTOs
{
    public class UpdateUserAndRolesDto
    {
        public required UpdateUserDto User { get; set; }
        public required string[] Roles { get; set; }
    }
}
