namespace CLIMenu
{
    /// <summary>
    /// Represents the base class for creating a command-line menu.
    /// Provides functionality for adding options, submenus, and handling user interaction.
    /// </summary>
    public abstract class MenuBase
    {
        /// <summary>
        /// Gets the display name of the menu.
        /// Must be overridden in derived classes.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the menu.
        /// Must be overridden in derived classes.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Reference to the parent menu (null if this is the root).
        /// </summary>
        public MenuBase? Parent { get; private set; }

        private readonly Dictionary<string, MenuOption> _options = new(StringComparer.OrdinalIgnoreCase);
        private bool _exitRequested;

        /// <summary>
        /// Adds a synchronous option to the menu.
        /// </summary>
        protected void AddOption(string key, string label, Action action)
        {
            _options[key] = new MenuOption(action) { Label = label };
        }

        /// <summary>
        /// Adds an asynchronous option to the menu.
        /// </summary>
        protected void AddOption(string key, string label, Func<Task> asyncAction)
        {
            _options[key] = new MenuOption(asyncAction) { Label = label };
        }

        /// <summary>
        /// Adds a submenu to the current menu.
        /// Selecting this option will display the submenu.
        /// </summary>
        protected void AddSubMenu(string key, MenuBase submenu)
        {
            submenu.Parent = this; // link back to parent

            _options[key] = new MenuOption(async () => await submenu.ShowAsync())
            {
                Label = submenu.Name
            };
        }

        /// <summary>
        /// Adds an exit option to the menu.
        /// Selecting this option will exit the current menu loop.
        /// </summary>
        protected void AddExitOption(string key = "0", string label = "Exit")
        {
            _options[key] = new MenuOption(() => _exitRequested = true) { Label = label };
        }

        /// <summary>
        /// Builds the breadcrumb-style path of the current menu.
        /// </summary>
        private string GetMenuPath()
        {
            var stack = new Stack<string>();
            var current = this;

            while (current != null)
            {
                stack.Push(current.Name);
                current = current.Parent!;
            }

            return string.Join(" > ", stack);
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
                Console.WriteLine($"=== {GetMenuPath()} ===");
                Console.WriteLine("");

                if (!string.IsNullOrWhiteSpace(Description))
                {
                    Console.WriteLine(Description);
                    Console.WriteLine("");
                }

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
