using CLIMenu;
using Identity.Core.Services;

namespace Identity.CLI
{
    public class MainMenu : MenuBase
    {
        public override string Name => "Main Menu";

        public override string Description => "Identity CLI";

        public MainMenu(IUserService userService, IUserValidationService userValidationService)
        {
            AddSubMenu("user", new UserMenu(userService, userValidationService));
        }
    }
}
