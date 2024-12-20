using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis.Formatting;

namespace CodeRule.Core
{
    public class Runner
    {
        private string SolutionFilePath { get; set; } = string.Empty;
        private List<string> ExcludeProjectNames { get; set; } = [];
        private string OutPutFileName { get; set; } = "violations.csv";
        private string OutPutDirectory { get; set; } = Directory.GetCurrentDirectory();
        private bool RunCodeFixes { get; set; } = false;

        public void AddRunCodeFixes(bool runCodeFixes)
        {
            RunCodeFixes = runCodeFixes;
        }

        public void AddOutputFileName(string outputFileName)
        {
            OutPutFileName = outputFileName;
        }

        public void AddOutputDirectory(string outputDirectory)
        {
            OutPutDirectory = outputDirectory;
        }

        public void AddExcludedProjectNames(string projectName)
        {
            ExcludeProjectNames.Add(projectName);
        }

        public void AddSolutionFilePath(string solutionPath)
        {
            SolutionFilePath = solutionPath;
        }

        public void Run(string[] args)
        {
            if(args.Length > 0) ConfigureOptionsWithArgs(args);

            if (string.IsNullOrWhiteSpace(SolutionFilePath) || !File.Exists(SolutionFilePath)) throw new ArgumentException("No valid solution file path was provided to run against.");
            if (!OutPutFileName.EndsWith(".csv")) throw new ArgumentException("Output file name must have a .csv extension.");
            if(!Directory.Exists(OutPutDirectory)) throw new ArgumentException("Output file directory dose not exist.");

            var allProjects = SolutionFile.Parse(SolutionFilePath);

            List<string> filteredProjectDirectoryPaths = [];

            foreach (var pair in allProjects.ProjectsByGuid)
            {
                var projType = pair.Value.ProjectType;
                var projectName = pair.Value.ProjectName;

                if (projType is SolutionProjectType.SolutionFolder) continue;
                if (ExcludeProjectNames.Contains(projectName)) continue;

                string parentDirectory = Path.GetDirectoryName(pair.Value.AbsolutePath)!;
                filteredProjectDirectoryPaths.Add(parentDirectory);
            }

            var rules = ReflectionHelper.FindAllCodeRules();
            var fixes = ReflectionHelper.FindAllCodeFixes();

            Parallel.ForEach(filteredProjectDirectoryPaths, projectPath =>
            {
                Analyze(projectPath, rules, fixes);
            });

            GenerateViolationsCsv(rules);
        }


        private void ConfigureOptionsWithArgs(string[] args)
        {
            foreach (var arg in args)
            {
                var argumentParts = arg.Split('=', 2);

                if (argumentParts.Length != 2 || string.IsNullOrWhiteSpace(argumentParts[1]))
                {
                    throw new ArgumentException($"Invalid argument format: {arg}");
                }

                var key = argumentParts[0];
                var value = argumentParts[1];

                switch (key)
                {
                    case "--solutionFilePath":
                        SolutionFilePath = value;
                        break;

                    case "--excludeProjects":
                        ExcludeProjectNames = value
                            .Split(',')
                            .Select(project => project.Trim())
                            .ToList();
                        break;

                    case "--outputFileName":
                        OutPutFileName = value;
                        break;

                    case "--outputDirectory":
                        OutPutDirectory = value;
                        break;

                    case "--runCodeFixes":
                        RunCodeFixes = bool.Parse(value);
                        break;

                    default:
                        throw new ArgumentException($"Unsupported argument: {key}");
                }
            }
        }

        public void Analyze(string projectPath, List<CodeRule> rules, List<CodeFix> codeFixes)
        {
            var allFilePathsInProject = GetAllFilePathsFromProject(projectPath);

            foreach (var filePath in allFilePathsInProject)
            {
                var rootNode = GetFileRootNode(filePath);

                foreach (var rule in rules)
                {
                    rule.Analyze(rootNode, filePath);
                }

                if (RunCodeFixes)
                {
                    foreach (var fix in codeFixes)
                    {
                        rootNode = fix.ApplyFix(rootNode);
                    }

                    rootNode = NormalizeNode(rootNode);

                    File.WriteAllText(filePath, rootNode.ToFullString());
                }
            }
        }

        private static SyntaxNode NormalizeNode(SyntaxNode rootNode)
        {
            return Formatter.Format(rootNode, new AdhocWorkspace());
        }

        private static List<string> GetAllFilePathsFromProject(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                throw new DirectoryNotFoundException($"The specified directory does not exist: {projectPath}");
            }

            string[] excludedDirectories =
            [
                "bin",
                "obj",
                "Debug",
                "Release",
                "packages",
                ".vs",
                "node_modules",
                "TestResults",
                ".git"
            ];

            var files = Directory.EnumerateFiles(projectPath, "*.cs", SearchOption.AllDirectories)
                .Where(file => !excludedDirectories.Any(dir =>
                    file.Contains($"{Path.DirectorySeparatorChar}{dir}{Path.DirectorySeparatorChar}") ||
                    file.Contains($"/{dir}/") || 
                    file.Contains($"\\{dir}\\")))
                .ToList();

            return files;
        }

        public static SyntaxNode GetFileRootNode(string filePath)
        {
            string sourceText = File.ReadAllText(filePath);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
                sourceText,
                path: filePath,
                options: new CSharpParseOptions(LanguageVersion.Latest)
            );

            var rootNode = syntaxTree.GetRoot();

            return rootNode;
        }

        private void GenerateViolationsCsv(List<CodeRule> rules)
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

            string filePath = Path.Combine(OutPutDirectory, OutPutFileName);
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
