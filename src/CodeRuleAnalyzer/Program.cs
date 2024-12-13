/*
 
This is expexted to be run in a solution as a project itself.

- It will get all other c sharp projects and nalyse them based on our standards.
- Produce console output of violations and what file, project and speficic code and line
 
 */


// Get the path to your solution file
string solutionFilePath = @"C:\dev\PCMS\PoliceCaseManagement.sln";

if (!File.Exists(solutionFilePath))
{
    Console.WriteLine("Solution file not found!");
    return;
}

// Get the directory containing the solution file
string solutionDirectory = Path.GetDirectoryName(solutionFilePath)!;
if (solutionDirectory is null)
{
    Console.WriteLine("Solution DIR null.");
    return;
};

// Ensure 'src' is included in the path resolution
string srcDirectory = Path.Combine(solutionDirectory, "src");

if (!Directory.Exists(srcDirectory))
{
    Console.WriteLine("'src' directory not found!");
    return;
}

// Read all lines from the solution file
var lines = File.ReadAllLines(solutionFilePath);
var projectPaths = new List<string>();

foreach (var line in lines)
{
    // Check for project entries in the solution file
    if (line.StartsWith("Project("))
    {
        // Extract the project path from the line
        var parts = line.Split('"');
        if (parts.Length > 3)
        {
            var relativePath = parts[3];

            // Combine with the 'src' directory to ensure the structure is correct
            var projectPath = Path.GetFullPath(Path.Combine(srcDirectory, relativePath));
            projectPaths.Add(projectPath);
        }
    }
}

// Output all project paths
Console.WriteLine("Projects in solution:");
foreach (var projectPath in projectPaths)
{
    Console.WriteLine(projectPath);
}


// Run

void Run(string)
{

}
