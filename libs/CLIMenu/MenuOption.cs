namespace CLIMenu
{
    /// <summary>
    /// Represents an option within a <see cref="MenuBase"/>.
    /// Can be either synchronous or asynchronous.
    /// </summary>
    public class MenuOption
    {
        /// <summary>
        /// Gets or sets the label displayed for this menu option.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets the synchronous action for this option, if applicable.
        /// </summary>
        public Action? SyncAction { get; }

        /// <summary>
        /// Gets the asynchronous action for this option, if applicable.
        /// </summary>
        public Func<Task>? AsyncAction { get; }

        /// <summary>
        /// Gets a value indicating whether this option is asynchronous.
        /// </summary>
        public bool IsAsync => AsyncAction != null;

        /// <summary>
        /// Gets the name of the underlying method associated with this option.
        /// </summary>
        public string ActionName => IsAsync ? AsyncAction!.Method.Name : SyncAction!.Method.Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuOption"/> class with a synchronous action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public MenuOption(Action action) => SyncAction = action;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuOption"/> class with an asynchronous action.
        /// </summary>
        /// <param name="asyncAction">The asynchronous action to execute.</param>
        public MenuOption(Func<Task> asyncAction) => AsyncAction = asyncAction;
    }
}
