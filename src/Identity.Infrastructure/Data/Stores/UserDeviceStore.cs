using Identity.Infrastructure.Data.Models;
using Shared.Utils;

namespace Identity.Infrastructure.Data.Stores
{
    public class UserDeviceStore(IdentityApplicationDbContext dbContext) : IUserDeviceStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<UserDevice> UserDevices => throw new NotImplementedException();

        public Task<UserDevice?> GetUserDeviceByIdAsync(ApplicationUser user, string userDeviceId)
        {
            throw new NotImplementedException();
        }

        public Task SetUserDevice(ApplicationUser user, UserDevice userDevice)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Succeed, ICollection<string> errors)> StoreUserDevice(ApplicationUser user, UserDevice userDevice)
        {
            throw new NotImplementedException();
        }
    }
}
