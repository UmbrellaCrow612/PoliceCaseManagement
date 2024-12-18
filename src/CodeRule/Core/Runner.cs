using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Construction;
using System.Data;

namespace CodeRule.Core
{
    public class Runner
    {
        private string SolutionFilePath { get; set; } = string.Empty;

        public void AddSolutionFilePath(string solutionPath)
        {
            SolutionFilePath = solutionPath;
        }

        public string GetSolutionFilePath()
        {
            return SolutionFilePath;
        }

        public void Run()
        {
            if (string.IsNullOrWhiteSpace(SolutionFilePath) || !File.Exists(SolutionFilePath)) throw new ArgumentException("No valid solution file path was provided to run against.");

            var allProjects = SolutionFile.Parse(SolutionFilePath);

            List<string> filteredProjectDirectoryPaths = [];

            foreach (var pair in allProjects.ProjectsByGuid)
            {
                var projType = pair.Value.ProjectType;

                if (projType is not SolutionProjectType.SolutionFolder) // we dont want to add solution folders as these contain all projects already
                {
                    string parentDirectory = Path.GetDirectoryName(pair.Value.AbsolutePath)!;
                    filteredProjectDirectoryPaths.Add(parentDirectory);
                }
            }

            var rules = ReflectionHelper.FindAllCodeRules();

            Parallel.ForEach(filteredProjectDirectoryPaths, projectPath =>
            {
                Analyze(projectPath, rules);
            });

            GenerateViolationsCsv(rules);
        }

        private void Analyze(string projectPath, List<CodeRule> rules)
        {
            var allFilePathsInProject = GetAllFilePathsFromProject(projectPath);

            foreach (var filePath in allFilePathsInProject)
            {
                var nodes = ReadFileAndConvertToNodes(filePath);

                foreach (var node in nodes)
                {
                    foreach (var rule in rules)
                    {
                        rule.Analyze(node, filePath);
                    }
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

        public static List<SyntaxNode> ReadFileAndConvertToNodes(string filePath)
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

        private static void CollectNodes(SyntaxNode node, List<SyntaxNode> nodeList)
        {
            nodeList.Add(node);

            foreach (var childNode in node.ChildNodes())
            {
                CollectNodes(childNode, nodeList);
            }
        }

        private static void GenerateViolationsCsv(List<CodeRule> rules)
        {
            var allViolations = new List<Violation>();
            foreach (var rule in rules)
            {
                allViolations.AddRange(rule.GetViolations());
            }

            var csvLines = new List<string>
                {
                    "Rule Class Name,File Path,Violating Token,Line Number, Column Number, Severity,Violation Description"
                };

            var violationsByRule = allViolations
                .GroupBy(v => rules.First(r => r.GetViolations().Contains(v)).GetRuleClassName())
                .ToList();

            foreach (var ruleViolations in violationsByRule)
            {
                foreach (var violation in ruleViolations)
                {
                    string csvLine = $"{ruleViolations.Key}," +
                                     $"\"{violation.FilePath}\"," +
                                     $"\"{violation.ViolatingToken}\"," +
                                     $"\"{violation.LineNumber}\"," +
                                     $"\"{violation.ColumnNumber}\"," +
                                     $"{violation.Severity}," +
                                     $"\"{violation.ViolationDescription}\"";
                    csvLines.Add(csvLine);
                }
            }

            string filePath = "test.csv";
            if (File.Exists(filePath))
            {
                File.AppendAllLines(filePath, csvLines.Skip(1));
            }
            else
            {
                File.WriteAllLines(filePath, csvLines);
            }
        }


    }
}
