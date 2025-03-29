namespace Identity.API.DTOs
{
    /// <summary>
    /// Dto to send across all the new roles a user has - note you need to also send the current roles with it
    /// for example if they had the role of admin and you dont make any changes then send admin back to here
    /// leaving it emoty means you wish the user to have no roles and hence the roles will be removed 
    /// vice versa if they where admin and you add another role like officer and send 
    /// [admin, officer] we will add the role of officer to them
    /// </summary>
    public class UpdateUserRolesDto
    {
        public required string[] Roles { get; set; }
    }
}
