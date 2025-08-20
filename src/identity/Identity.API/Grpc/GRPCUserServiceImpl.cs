using Grpc.Core;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using User.V1;

namespace Identity.API.Grpc
{
    /// <summary>
    /// Grpc impl of the proto file and exposes it for consumers
    /// </summary>
    /// <param name="authService"></param>
    [Authorize]
    public class GRPCUserServiceImpl(IUserService userService) : UserService.UserServiceBase
    {
        private readonly IUserService _userService = userService;

        public override async Task<DoesUserExistResponse> DoesUserExist(DoesUserExistRequest request, ServerCallContext context)
        {
            bool exists = await _userService.ExistsAsync(request.UserId);

            return new DoesUserExistResponse { Exists = exists };
        }


        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _userService.FindByIdAsync(request.UserId) ?? throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.UserId} not found"));
            return new GetUserByIdResponse
            {
                Email = user.Email,
                UserId = user.Id,
                Username = user.UserName,
            };
        }
    }
}
