using CodeRuleAnalyzer;

string solutionPath = @"C:\dev\PCMS";

var runner = new Runner();

runner.AddSolutionPath(solutionPath);

runner.Run();
