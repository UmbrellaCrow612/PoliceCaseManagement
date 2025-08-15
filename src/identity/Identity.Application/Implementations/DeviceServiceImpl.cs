using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IDeviceService"/> - test this, as well when using it else where only use the <see cref="IDeviceService"/>
    /// interface not this class
    /// </summary>
    public class DeviceServiceImpl(IdentityApplicationDbContext dbContext, IDeviceIdentificationGenerator deviceIdentificationGenerator) : IDeviceService
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly IDeviceIdentificationGenerator _deviceIdentificationGenerator = deviceIdentificationGenerator;

        public async Task<Device?> FindByIdAsync(string deviceId)
        {
            return await _dbContext.Devices.FindAsync(deviceId);
        }

        public async Task<Device?> GetDeviceAsync(string userId, DeviceInfo info)
        {
            var deviceId = _deviceIdentificationGenerator.GenerateId(userId, info);

            return await _dbContext.Devices.FindAsync(deviceId);
        }
    }
}
