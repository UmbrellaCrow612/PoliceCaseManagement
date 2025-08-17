namespace Identity.API.DTOs
{
    public class UserMeResponseDto
    {
        public required UserDto User { get; set; }
        public required List<string> Roles { get; set; }
    }
}
