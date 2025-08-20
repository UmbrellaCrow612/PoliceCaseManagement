using Identity.Core.Models;
using Identity.Core.Services;
using Results.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IUserValidationService"/> - test this, as well when using it else where only use the <see cref="IUserValidationService"/>
    /// interface not this class
    /// </summary>
    public class UserValidationServiceImpl : IUserValidationService
    {
        public IResult Validate(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IResult ValidateEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IResult ValidatePassword(string password)
        {
            throw new NotImplementedException();
        }

        public IResult ValidatePhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public IResult ValidateUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
