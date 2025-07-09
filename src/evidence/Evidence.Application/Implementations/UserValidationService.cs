using User.V1;

namespace Evidence.Application.Implementations
{
    /// <summary>
    /// Helper to validate user information
    /// </summary>
    public class UserValidationService(UserService.UserServiceClient userServiceClient)
    {
        public async Task<bool> DoseUserExist(string userId)
        {
            var response = await userServiceClient.DoesUserExistAsync(new DoesUserExistRequest { UserId = userId });
            return response.Exists;
        }

        public async Task<GetUserByIdResponse> GetUserById(string userId)
        {
            return await userServiceClient.GetUserByIdAsync(new GetUserByIdRequest { UserId = userId });
        }
    }
}
