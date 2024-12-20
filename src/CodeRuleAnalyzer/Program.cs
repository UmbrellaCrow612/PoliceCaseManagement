using CodeRule.Core;

var runner = new Runner();

runner.AddExcludedProjectName("Cases.API");
runner.AddExcludedProjectName("Cases.Infrastructure");
runner.AddExcludedProjectName("SMS.Service");
runner.AddExcludedProjectName("Shared");
runner.AddExcludedProjectName("Identity.API");
runner.AddExcludedProjectName("Evidence.Infrastructure");
runner.AddExcludedProjectName("Email.Service");
runner.AddExcludedProjectName("Entity.API");
runner.AddExcludedProjectName("Entity.Infrastructure");
runner.AddExcludedProjectName("Evidence.API");

runner.Run(args);