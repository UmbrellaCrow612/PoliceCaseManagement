namespace CodeRuleAnalyzer.Core
{
    internal static class Logger
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            Debug,
            Critical
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            // Set console color based on log level
            ConsoleColor originalColor = Console.ForegroundColor;

            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan; // Light blue for info
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow; // Yellow for warnings
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red; // Red for errors
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Green; // Green for debug
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Magenta; // Magenta for critical issues
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White; // Default white
                    break;
            }

            // Output the message with the timestamp and log level
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");

            // Reset console color to the original
            Console.ForegroundColor = originalColor;
        }
    }
}
