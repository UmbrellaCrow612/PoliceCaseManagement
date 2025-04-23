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
            AddRule(x => string.IsNullOrWhiteSpace(x.ReportingOfficerId), "ReportingOfficerId must not be empty or whitespace if provided");

            AddRule(x => x.CaseNumber.Length < 5, "Case number must be grater than 5 characters long");
        }
    }
}
