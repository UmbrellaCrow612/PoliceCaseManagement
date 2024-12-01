using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IDeviceInfoStore
    {
        IQueryable<DeviceInfo> DeviceInfos { get; }

        Task SetDeviceInfo(DeviceInfo deviceInfo);
        Task StoreDeviceInfo(DeviceInfo deviceInfo);
        Task<DeviceInfo?> GetDeviceInfoByIdAsync(string deviceInfoId);
    }
}
