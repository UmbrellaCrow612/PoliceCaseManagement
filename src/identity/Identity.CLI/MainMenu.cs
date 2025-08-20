using CLIMenu;
using Identity.Core.Services;

namespace Identity.CLI
{
    public class MainMenu : MenuBase
    {
        public MainMenu(IUserService userService) : base("Main Menu", "Welcome to the Identity CLI")
        {
            // Add an asynchronous option
            AddOption("1", "Find a user by there ID", async () =>
            {
                Console.WriteLine("Enter the ID of the user to find");

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
                } else
                {
                    Console.WriteLine("User found");
                    Console.WriteLine(user);
                }
            });
        }
    }
}
