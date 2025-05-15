using Grpc.Core;
using Identity.Core.Services;
using User.V1;

namespace Identity.API.Grpc
{

    public class UserServiceImpl(IAuthService authService) : UserService.UserServiceBase
    {
        private readonly IAuthService _authService = authService;

        public override async Task<DoesUserExistResponse> DoesUserExist(DoesUserExistRequest request, ServerCallContext context)
        {
            bool exists = await _authService.UserExists(request.UserId);

            return new DoesUserExistResponse { Exists = exists };
        }
    }
}
