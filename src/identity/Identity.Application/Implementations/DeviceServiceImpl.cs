using Identity.Application.Codes;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Results.Abstractions;

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

        public async Task<IResult> CreateAsync(ApplicationUser user, DeviceInfo deviceInfo)
        {
            var result = new Result();

            var deviceId = _deviceIdentificationGenerator.GenerateId(user.Id, deviceInfo);

            var deviceExists = await _dbContext.Devices.AnyAsync(x => x.Id == deviceId);
            if (deviceExists)
            {
                result.AddError(BusinessRuleCodes.DeviceExists, "Device already exists");
                return result;
            }

            var device = new Device
            {
                Id = deviceId,
                Name = deviceInfo.UserAgent,
                UserId = user.Id,
            };
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<bool> ExistsAsync(string userId, DeviceInfo info)
        {
            var deviceId = _deviceIdentificationGenerator.GenerateId(userId, info);

            return await _dbContext.Devices.AnyAsync(x => x.Id == deviceId);
        }

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
