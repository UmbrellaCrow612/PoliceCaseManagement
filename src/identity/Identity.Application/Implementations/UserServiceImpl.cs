using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IUserService"/> - test this, as well when using it else where only use the <see cref="IUserService"/>
    /// interface not this class
    /// </summary>
    public class UserServiceImpl(IdentityApplicationDbContext dbContext) : IUserService
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailTaken(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPhoneNumberTaken(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUsernameTaken(string username)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<ApplicationUser>> SearchAsync(SearchUserQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
