using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer
{
    public class Runner
    {
        private string SolutionPath { get; set; } = string.Empty;

        private static ICollection<string> GetProjectDirectoryPathsFromSolutionFile(string solutionFilePath)
        {
            var projectDirectories = new HashSet<string>();

            if (!File.Exists(solutionFilePath))
            {
                throw new FileNotFoundException($"Solution file not found at {solutionFilePath}");
            }

            var lines = File.ReadAllLines(solutionFilePath);

            foreach (var line in lines)
            {
                if (line.StartsWith("Project("))
                {
                    var parts = line.Split(',');
                    if (parts.Length > 1)
                    {
                        var projectPath = parts[1].Trim().Trim('"');
                        var fullProjectPath = Path.Combine(Path.GetDirectoryName(solutionFilePath)!, projectPath);

                        if (File.Exists(fullProjectPath))
                        {
                            var projectDirectory = Path.GetDirectoryName(fullProjectPath);
                            if (projectDirectory != null)
                            {
                                projectDirectories.Add(projectDirectory);
                            }
                        }
                    }
                }
            }

            return projectDirectories;
        }

        private static (bool isFound, string? fileName, string? filePath) GetSolutionFile(string solutionPath)
        {
            if (Directory.Exists(solutionPath))
            {
                var solutionFiles = Directory.GetFiles(solutionPath, "*.sln", SearchOption.TopDirectoryOnly);

                if (solutionFiles.Length > 0)
                {
                    string fullPath = solutionFiles[0];
                    string fileName = Path.GetFileName(fullPath);
                    return (true, fileName, fullPath);
                }
            }

            return (false, null, null);
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

            ICollection<string> projectPaths = GetProjectDirectoryPathsFromSolutionFile(filePath);

            foreach ( var projectPath in projectPaths )
            {
                Analyze(projectPath);
            }
        }



        private void Analyze(string projectPath)
        {
            var allFilePathsInProject = GetAllFilePathsFromProject(projectPath);

            foreach (var filePath in allFilePathsInProject)
            {
                var nodes = ReadFileAndConvertToNodes(filePath);

                foreach (var node in nodes)
                {
                    Analyzers.AnalyzeNode(node, filePath);
                }
            }
        }

        private static ICollection<string> GetAllFilePathsFromProject(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                throw new DirectoryNotFoundException($"The specified directory does not exist: {projectPath}");
            }

            return new List<string>(Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories));
        }

        public List<SyntaxNode> ReadFileAndConvertToNodes(string filePath)
        {
            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fileStream);
                string fileContent = reader.ReadToEnd();

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileContent);

                SyntaxNode rootNode = syntaxTree.GetRoot();

                var nodes = new List<SyntaxNode>();
                CollectNodes(rootNode, nodes);

                return nodes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return []; // Return an empty list if there's an error
            }
        }

        private void CollectNodes(SyntaxNode node, List<SyntaxNode> nodeList)
        {
            nodeList.Add(node);

            foreach (var childNode in node.ChildNodes())
            {
                CollectNodes(childNode, nodeList);
            }
        }
    }
}
