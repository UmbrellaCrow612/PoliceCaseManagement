using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Identity.Infrastructure.Data.Stores
{
    public class UserDeviceStore(IdentityApplicationDbContext dbContext) : IUserDeviceStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<UserDevice> UserDevices => throw new NotImplementedException();

        public async Task<UserDevice?> GetUserDeviceByIdAsync(ApplicationUser user, string deviceId)
        {
            return  await _dbcontext.UserDevices.FirstOrDefaultAsync(x => x.Id == deviceId && x.UserId == user.Id);
        }

        public async Task SetUserDevice(ApplicationUser user, UserDevice userDevice)
        {
            var inContext = EfHelper.ExistsInContext(_dbcontext,userDevice); // fetched from the user device store so it dose exist
            if (!inContext)
            {
                await _dbcontext.UserDevices.AddAsync(userDevice);
            }
        }

        public Task<(bool Succeed, ICollection<string> errors)> StoreUserDevice(ApplicationUser user, UserDevice userDevice)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UserDevice userDevice)
        {
            if (!EfHelper.ExistsInContext(_dbcontext, userDevice)) throw new Exception();

            _dbcontext.UserDevices.Update(userDevice);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
