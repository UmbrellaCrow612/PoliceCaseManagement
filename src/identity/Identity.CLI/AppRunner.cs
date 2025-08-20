using Identity.Core.Services;

namespace Identity.CLI
{
    /// <summary>
    /// Represents the console CLI app - added to the DI then read and runs the main thread - allows early termination for service issues
    /// </summary>
    internal class AppRunner
        (
            IUserService userService,
            IUserValidationService userValidationService
        )
    {
        /// <summary>
        /// Runs the main thread while loop
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync() {
            do
            {
              var mainMenu = new MainMenu(userService, userValidationService);

              await mainMenu.ShowAsync();

            } while (true);
        }
    }
}
