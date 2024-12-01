using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public class DeviceInfoStore(IdentityApplicationDbContext dbContext) : IDeviceInfoStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<DeviceInfo> DeviceInfos => _dbcontext.DeviceInfos.AsQueryable();

        public Task<DeviceInfo?> GetDeviceInfoByIdAsync(string deviceInfoId)
        {
            throw new NotImplementedException();
        }

        public async Task SetDeviceInfo(DeviceInfo deviceInfo)
        {
            await _dbcontext.DeviceInfos.AddAsync(deviceInfo);
        }

        public Task StoreDeviceInfo(DeviceInfo deviceInfo)
        {
            throw new NotImplementedException();
        }
    }
}
