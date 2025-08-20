using Identity.Core.Services;

namespace Identity.CLI
{
    /// <summary>
    /// Represents the console CLI app - added to the DI then read and runs the main thread - allows early termination for service issues
    /// </summary>
    internal class AppRunner
        (
            IUserService userService
        )
    {
        /// <summary>
        /// Runs the main thread while loop
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync() {
            Console.WriteLine("Identity CLI started. Type 'exit' to quit.");
            Console.WriteLine("Or type a user ID to search for that user.\n");

            string? input;
            do
            {
                Console.Write("> ");
                input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                    break;

                // Try to find the user by ID
                var user = await userService.FindByIdAsync(input);

                if (user != null)
                {
                    Console.WriteLine($"[FOUND] User ID: {user.Id}, Username: {user.UserName}, Email: {user.Email}");
                }
                else
                {
                    Console.WriteLine($"[NOT FOUND] No user exists with ID: {input}");
                }
            } while (true);

            Console.WriteLine("Shutting down...");
        }
    }
}
