using Microsoft.CodeAnalysis.CSharp.Syntax;
using Style.Core;

namespace Style.StyleRuleDefinitions
{
    public class CSharpDefinitions
    {
        public static StyleRule ParameterNamingConvention = new(
           "Parameter Naming Convention",
           node =>
           {
               if (node is MethodDeclarationSyntax method)
               {
                   foreach (var parameter in method.ParameterList.Parameters)
                   {

                       if (parameter.Identifier.Text.ToLower() == "id")
                       {
                           return new StyleViolation(
                               "Parameter Naming Convention",
                               $"Parameter '{parameter.Identifier.Text}' should be more descriptive.",
                               ViolationSeverity.Warning,
                               parameter
                           );
                       }
                   }
               }
               return null;
           }
       );
    }
}
