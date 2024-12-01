using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IUserDeviceStore
    {
        IQueryable<UserDevice> UserDevices { get; }

        Task<UserDevice?> GetUserDeviceByIdAsync(ApplicationUser user, string userDeviceId);
        Task SetUserDevice(ApplicationUser user, UserDevice userDevice);
        Task<(bool Succeed, ICollection<string> errors)> StoreUserDevice(ApplicationUser user, UserDevice userDevice);
    }
}
