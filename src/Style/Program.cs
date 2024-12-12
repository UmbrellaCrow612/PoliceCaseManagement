
/*
 
 
This project is run and will produce a report of all violations of rules and styles we define.
 
 */


using Style.StyleRuleDefinitions;
using Style.Styles;

var styleChecker = new CSharpStyleChecker();

// Add predefined rules
styleChecker.AddRule(CSharpDefinitions.ParameterNamingConvention);

// Check a directory
styleChecker.CheckDirectory(@"C:\dev\PCMS\src\Style");

// Print out violations
styleChecker.PrintViolations();