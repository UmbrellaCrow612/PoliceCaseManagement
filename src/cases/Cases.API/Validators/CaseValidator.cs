using Cases.Core.Models;
using Validator;

namespace Cases.API.Validators
{
    /// <summary>
    /// Validation to run against a <see cref="Case"/> before we create it - typically from dto to a model state
    /// </summary>
    public class CaseValidator : Validator<Case>
    {
        public CaseValidator() 
        {
            AddRule(
            x => x.CaseNumber is not null && x.CaseNumber.Trim() == string.Empty,
            "Case number must not be empty or whitespace if provided"
            );

            AddRule(x => string.IsNullOrWhiteSpace(x.ReportingOfficerId), "ReportingOfficerId must not be empty or whitespace if provided");
        }
    }
}
