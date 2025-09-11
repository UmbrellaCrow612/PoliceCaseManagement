using Identity.Application.Codes;
using Identity.Core.Models;
using Identity.Core.RegexHelpers;
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
            var result = new Result();

            var emailResult = ValidateEmail(user.Email);
            if (!emailResult.Succeeded)
            {
                result.Errors.AddRange(emailResult.Errors);
            }

            var phoneResult = ValidatePhoneNumber(user.PhoneNumber);
            if (!phoneResult.Succeeded)
            {
                result.Errors.AddRange(phoneResult.Errors);
            }

            var usernameResult = ValidateUsername(user.UserName);
            if (!usernameResult.Succeeded)
            {
                result.Errors.AddRange(usernameResult.Errors);
            }

            if (result.Errors.Count > 0)
            {
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public IResult ValidateEmail(string email)
        {
            var result = new Result();
            if (string.IsNullOrWhiteSpace(email))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Email empty");
                return result;
            }

            if (!UserRegex.EmailRegex().IsMatch(email))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Email format is invalid.");
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public IResult ValidatePassword(string password)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(password))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Password cannot be empty.");
                return result;
            }

            if (!UserRegex.PasswordRegex().IsMatch(password))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Password must be at least 8 characters long and include at least one letter and one number.");
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public IResult ValidatePhoneNumber(string phoneNumber)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                result.AddError(BusinessRuleCodes.ValidationError,"Phone number cannot be empty.");
                return result;
            }

            if (!UserRegex.PhoneNumberRegex().IsMatch(phoneNumber))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Phone number must start with '+' followed by country code and number.");
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public IResult ValidateUsername(string username)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(username))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Username cannot be empty.");
                return result;
            }

            if (username.Length < 3 || username.Length > 20)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Username must be between 3 and 20 characters long.");
            }

            if (!UserRegex.UsernameRegex().IsMatch(username))
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Username can only contain letters, numbers, and allowed special characters (_-@!).");
            }

            if (result.Errors.Count == 0)
            {
                result.Succeeded = true;
            }

            return result;
        }
    }
}
