using Evidence.Core.ValueObjects;
using Validator;

namespace Evidence.API.Validators
{
    public class SearchEvidenceQueryValidator : Validator<SearchEvidenceQuery>
    {
        public SearchEvidenceQueryValidator()
        {
            AddRule(x => x.PageNumber < 0, "Page number cannot be below 0");

            AddRule(x => x.PageSize > 50, "Page size cannot be above 50 items per page");

            AddRule(x => x.PageSize <= 0, "Page size needs to be greater than 0");
        }
    }
}
