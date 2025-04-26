using Cases.Core.Models;
using Cases.Core.ValueObjects;
using Validator;

namespace Cases.API.Validators
{
    public class SearchCasesQueryValidator : Validator<SearchCasesQuery>
    {
        public SearchCasesQueryValidator()
        {
            AddRule(x => x.PageSize > 100, "Page size cannot be greater than 100");
            AddRule(x => x.PageSize < 0, "Page size cannot be less than 0");

            AddRule(x => x.PageNumber < 0, "Page number cannot be less than 0");

            AddRule(
             x => x.Status.HasValue && !Enum.IsDefined(x.Status.Value),
             $"Provided status is not a valid {nameof(CaseStatus)} value."
         );
        }
    }
}
