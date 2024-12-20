using CodeRule.Core;

var runner = new Runner();

runner.AddExcludedProjectNames("Cases.API");
runner.AddExcludedProjectNames("Cases.Infrastructure");
runner.AddExcludedProjectNames("SMS.Service");
runner.AddExcludedProjectNames("Shared");
runner.AddExcludedProjectNames("Identity.API");
runner.AddExcludedProjectNames("Evidence.Infrastructure");
runner.AddExcludedProjectNames("Email.Service");
runner.AddExcludedProjectNames("Entity.API");
runner.AddExcludedProjectNames("Entity.Infrastructure");
runner.AddExcludedProjectNames("Evidence.API");

runner.Run(args);