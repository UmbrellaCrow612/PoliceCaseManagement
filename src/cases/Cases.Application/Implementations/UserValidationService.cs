using User.V1;

namespace Cases.Application.Implementations
{
    public class UserValidationService(UserService.UserServiceClient client)
    {
        private readonly UserService.UserServiceClient _client = client;

        public async Task<bool> DoesUserExistAsync(string userId)
        {
            var response = await _client.DoesUserExistAsync(new DoesUserExistRequest { UserId = userId });
            return response.Exists;
        }


        public async Task<GetUserByIdResponse> GetUserById(string userId)
        {
            return await _client.GetUserByIdAsync(new GetUserByIdRequest { UserId = userId });
        }
    }
}
