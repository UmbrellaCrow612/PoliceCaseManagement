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


        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _authService.GetUserByIdAsync(request.UserId) ?? throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.UserId} not found"));
            return new GetUserByIdResponse
            {
                Email = user.Email,
                UserId = user.Id,
                Username = user.UserName,
            };
        }
    }
}
