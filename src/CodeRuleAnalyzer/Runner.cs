namespace CodeRuleAnalyzer
{
    public class Runner
    {
        private string SolutionPath { get; set; } = string.Empty;

        private ICollection<string> GetProjectPathsFromSolutionFile(string solutionFilePath)
        {
            return []; // return all valid projects and there paths to them.
        }

        private (bool isFound, string? fileName, string? filePath) GetSolutionFile(string solutionPath)
        {
            return (false, "", "");
        }

        public void AddSolutionPath(string solutionPath)
        {
            SolutionPath = solutionPath;
        }

        public string GetSolutionPath()
        {
            return SolutionPath;
        }

        public void Run()
        {
            if (string.IsNullOrWhiteSpace(SolutionPath) || !Directory.Exists(SolutionPath)) throw new ArgumentException("No valid solution path was provided to run against.");

            var (isFound, fileName, filePath) = GetSolutionFile(SolutionPath);
            if (!isFound || string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Solution path dose not contain a solution file.");

            ICollection<string> projectPaths = GetProjectPathsFromSolutionFile(filePath);

            foreach ( var projectPath in projectPaths )
            {
                Analyze(projectPath);
            }
        }

        private void Analyze(string solutionFi)
        {

        }
    }
}
