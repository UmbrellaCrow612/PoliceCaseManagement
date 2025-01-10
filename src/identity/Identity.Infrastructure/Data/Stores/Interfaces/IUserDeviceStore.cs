using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface IUserDeviceStore
    {
        IQueryable<UserDevice> UserDevices { get; }

        Task UpdateAsync(UserDevice userDevice);
        Task<UserDevice?> GetUserDeviceByIdAsync(string deviceId);
        Task SetUserDevice(ApplicationUser user, UserDevice userDevice);
        Task<(bool Succeed, ICollection<string> errors)> StoreUserDevice(ApplicationUser user, UserDevice userDevice);
    }
}
