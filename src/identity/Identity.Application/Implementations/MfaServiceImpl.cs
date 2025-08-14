using Identity.Core.Services;
using Identity.Core.ValueObjects;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IMfaService"/> - test this, as well when using it else where only use the <see cref="IMfaService"/>
    /// interface not this class
    /// </summary>
    public class MfaServiceImpl : IMfaService
    {
        public Task<MfaResult> SendMfaSmsAsync(string loginId, DeviceInfo deviceInfo)
        {
            throw new NotImplementedException();
        }

        public Task<VerifiedMfaResult> VerifyMfaSmsAsync(string loginId, string code, DeviceInfo deviceInfo)
        {
            throw new NotImplementedException();
        }

        public Task<VerifiedMfaResult> VerifyTotpAsync(string loginId, string code, DeviceInfo deviceInfo)
        {
            throw new NotImplementedException();
        }
    }
}
