using CodeRuleAnalyzer;

string solutionFilePath = @"C:\dev\PCMS\PoliceCaseManagement.sln";

var runner = new Runner();

runner.AddSolutionFilePath(solutionFilePath);

runner.Run();
