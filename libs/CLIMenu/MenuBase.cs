namespace CLIMenu
{
    /// <summary>
    /// Represents the base class for creating a command-line menu.
    /// Provides functionality for adding options, submenus, and handling user interaction.
    /// </summary>
    public abstract class MenuBase(string name, string description = "")
    {
        /// <summary>
        /// Gets the display name of the menu.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// Gets the description of the menu.
        /// </summary>
        public string Description { get; } = description;

        private readonly Dictionary<string, MenuOption> _options = new(StringComparer.OrdinalIgnoreCase);
        private bool _exitRequested;

        /// <summary>
        /// Adds a synchronous option to the menu.
        /// </summary>
        /// <param name="key">The input key that triggers the option.</param>
        /// <param name="label">The label displayed in the menu.</param>
        /// <param name="action">The action to execute when the option is selected.</param>
        protected void AddOption(string key, string label, Action action)
        {
            _options[key] = new MenuOption(action) { Label = label };
        }

        /// <summary>
        /// Adds an asynchronous option to the menu.
        /// </summary>
        /// <param name="key">The input key that triggers the option.</param>
        /// <param name="label">The label displayed in the menu.</param>
        /// <param name="asyncAction">The asynchronous action to execute when the option is selected.</param>
        protected void AddOption(string key, string label, Func<Task> asyncAction)
        {
            _options[key] = new MenuOption(asyncAction) { Label = label };
        }

        /// <summary>
        /// Adds a submenu to the current menu.
        /// Selecting this option will display the submenu.
        /// </summary>
        /// <param name="key">The input key that triggers the submenu.</param>
        /// <param name="submenu">The submenu instance to display.</param>
        protected void AddSubMenu(string key, MenuBase submenu)
        {
            _options[key] = new MenuOption(async () => await submenu.ShowAsync())
            {
                Label = submenu.Name
            };
        }

        /// <summary>
        /// Adds an exit option to the menu.
        /// Selecting this option will exit the current menu loop.
        /// </summary>
        /// <param name="key">The input key that triggers the exit option (default is "0").</param>
        /// <param name="label">The label displayed in the menu (default is "Exit").</param>
        protected void AddExitOption(string key = "0", string label = "Exit")
        {
            _options[key] = new MenuOption(() => _exitRequested = true) { Label = label };
        }

        /// <summary>
        /// Displays the menu and handles user input until the exit option is selected.
        /// </summary>
        public async Task ShowAsync()
        {
            _exitRequested = false;

            while (!_exitRequested)
            {
                Console.Clear();
                Console.WriteLine($"=== {Name} ===");
                Console.WriteLine("");
                if (!string.IsNullOrWhiteSpace(Description))
                {
                    Console.WriteLine(Description);
                }

                Console.WriteLine("");
                foreach (var kvp in _options)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value.Label}");
                }

                Console.WriteLine("");
                Console.Write("Choose an option: ");
                Console.WriteLine("");
                var input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(input) && _options.TryGetValue(input, out var option))
                {
                    try
                    {
                        if (option.IsAsync)
                            await option.AsyncAction!();
                        else
                            option.SyncAction!();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    if (!_exitRequested) // don't block if exiting
                    {
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Press any key to try again...");
                    Console.ReadKey();
                }
            }
        }
    }
}
