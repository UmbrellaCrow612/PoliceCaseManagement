using CodeRule.Core;

namespace CodeRule.Tests
{
    public class RunnerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var runner = new Runner();

            Assert.Equal(string.Empty, runner.CurrentWorkingSolutionFile());
            Assert.Empty(runner.CurrentExcludeProjectNames());
            Assert.Equal("violations.csv", runner.CurrentOutputFileName());
            Assert.Equal(Directory.GetCurrentDirectory(), runner.CurrentOutputDirectory());
            Assert.False(runner.CurrentRunCodeFixesMode());
        }

        [Fact]
        public void AddSolutionFilePath_ShouldSetSolutionFilePath()
        {
            var runner = new Runner();
            var solutionPath = "path/to/solution.sln";
            runner.AddSolutionFilePath(solutionPath);
            Assert.Equal(solutionPath, runner.CurrentWorkingSolutionFile());
        }

        [Fact]
        public void AddExcludedProjectName_ShouldAddProjectName()
        {
            var runner = new Runner();
            var projectName = "ProjectName";
            runner.AddExcludedProjectName(projectName);
            Assert.Contains(projectName, runner.CurrentExcludeProjectNames());
        }

        [Fact]
        public void AddOutputDirectory_ShouldSetOutputDirectory()
        {
            var runner = new Runner();
            var outputDirectory = "path/to/output";
            runner.AddOutputDirectory(outputDirectory);
            Assert.Equal(outputDirectory, runner.CurrentOutputDirectory());
        }

        [Fact]
        public void AddOutputFileName_ShouldSetOutputFileName()
        {
            var runner = new Runner();
            var outputFileName = "output.csv";
            runner.AddOutputFileName(outputFileName);
            Assert.Equal(outputFileName, runner.CurrentOutputFileName());
        }

        [Fact]
        public void AddRunCodeFixes_ShouldSetRunCodeFixes()
        {
            var runner = new Runner();
            runner.AddRunCodeFixes(true);
            Assert.True(runner.CurrentRunCodeFixesMode());
        }

        [Fact]
        public void AddRunCodeFixes_ShouldSetRunCodeFixesToFalse()
        {
            var runner = new Runner();
            runner.AddRunCodeFixes(false);
            Assert.False(runner.CurrentRunCodeFixesMode());
        }

        [Fact]
        public void AddExcludedProjectName_ShouldAddMultipleProjectNames()
        {
            var runner = new Runner();
            var projectNames = new List<string> { "Project1", "Project2" };
            foreach (var projectName in projectNames)
            {
                runner.AddExcludedProjectName(projectName);
            }
            Assert.Equal(projectNames, runner.CurrentExcludeProjectNames());
        }

        [Fact]
        public void AddExcludedProjectName_ShouldAddUniqueProjectNames()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                var runner = new Runner();
                var projectName = "ProjectName";
                runner.AddExcludedProjectName(projectName);
                runner.AddExcludedProjectName(projectName);
            });
        }
    }
}
