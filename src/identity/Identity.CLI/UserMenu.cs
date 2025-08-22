using CLIMenu;
using Identity.Core.Models;
using Identity.Core.Services;

namespace Identity.CLI
{
    internal class UserMenu : MenuBase
    {
        public override string Name => "User management";

        public override string Description => "User creation viewing etc";

        internal UserMenu(IUserService userService, IUserValidationService userValidationService)
        {
            AddOption("1", "Find a user by there ID", async () =>
            {
                Console.WriteLine("");
                Console.WriteLine("Enter the ID of the user to find");
                Console.WriteLine("");

                string? userId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userId))
                {
                    Console.Error.WriteLine("Invalid null or white space ID");
                    return;
                }

                var user = await userService.FindByIdAsync(userId);
                if (user is null)
                {
                    Console.WriteLine($"User : {userId} was not found");
                }
                else
                {
                    Console.WriteLine("User found");
                    Console.WriteLine(user);
                }
            });

            AddOption("2", "Create a user", async () =>
            {
                Console.WriteLine("");
                Console.WriteLine("Enter the fields required for a user creation");
                Console.WriteLine("");

                string userName = CLIHelpers.GetInput<string>(
                "Enter the user's username: ",
                [
                   input => !userValidationService.ValidateUsername(input).Succeeded
                ],
                "Username validation failed. Please try again."
                );

                string email = CLIHelpers.GetInput<string>(
                "Enter the user's email: ",
                [
                   input => !userValidationService.ValidateEmail(input).Succeeded
                ],
                "Email validation failed. Please try again."
                );

                string phoneNumber = CLIHelpers.GetInput<string>(
                "Enter the user's phone number: ",
                [
                   input => !userValidationService.ValidatePhoneNumber(input).Succeeded
                ],
                "Phone number validation failed. Please try again."
                );

                string password = CLIHelpers.GetInput<string>(
                "Enter the user's password: ",
                [
                   input => !userValidationService.ValidatePassword(input).Succeeded
                ],
                "Password validation failed. Please try again."
                );

                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                };

                var result = await userService.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine(user);
                }
            });

            AddExitOption();
        }
    }
}
