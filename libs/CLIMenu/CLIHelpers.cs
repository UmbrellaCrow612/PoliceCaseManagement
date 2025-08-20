namespace CLIMenu
{
    /// <summary>
    /// Helper methods for the CLI
    /// </summary>
    public static class CLIHelpers
    {
        /// <summary>
        /// Continuously prompts the user for input from the console until all provided validators pass.
        /// Converts the valid input to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert the user input to.</typeparam>
        /// <param name="prompt">The message displayed to the user when requesting input.</param>
        /// <param name="validators">
        /// A list of functions that each take a string input and return <c>true</c> if valid, <c>false</c> otherwise.
        /// The input must pass all validators to be accepted.
        /// </param>
        /// <param name="errorMessage">The message displayed if the input fails any of the validators.</param>
        /// <returns>The validated user input converted to type <typeparamref name="T"/>.</returns>
        public static T GetInput<T>(string prompt, List<Func<string, bool>> validators, string errorMessage)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                bool isValid = true;
                foreach (var validator in validators)
                {
                    if (validator(input))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    return (T)Convert.ChangeType(input, typeof(T));
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }

    }
}
