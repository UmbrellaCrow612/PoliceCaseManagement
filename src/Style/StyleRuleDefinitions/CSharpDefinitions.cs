using Microsoft.CodeAnalysis.CSharp.Syntax;
using Style.Core;

namespace Style.StyleRuleDefinitions
{
    public class CSharpDefinitions
    {
        public static StyleRule MethodNameStartsWithCapital = new(
           "Method Naming Convention",
           node => node is MethodDeclarationSyntax method && char.IsUpper(method.Identifier.Text[0])
               ? new StyleViolation(
                   "Method Naming Convention",
                   $"Method '{method.Identifier}' should start with a capital letter.",
                   ViolationSeverity.Warning,
                   method)
               : null
       );
    }
}
