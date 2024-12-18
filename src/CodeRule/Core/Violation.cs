using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public class Violation
    {
        public required string FilePath { get; set; }
        public required SyntaxToken ViolatingToken { get; set; }
        public required string LineNumber { get; set; }
        public required string ColumnNumber { get; set; }
        public required string ViolationDescription { get; set; } 
        public required string Severity { get; set; }
    }
}
