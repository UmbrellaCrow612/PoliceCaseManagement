namespace Identity.API.DTOs
{
    public class MeDto
    {
        public required UserDto User { get; set; }
        public required ICollection<string> Roles { get; set; }
    }
}
