using Evidence.Core.ValueObjects;
using Validator;

namespace Evidence.API.Validators
{
    /// <summary>
    /// Custom validation logic to tun on <see cref="Core.ValueObjects.SearchTagsQuery"/> to make sure they abide to some rules we define
    /// </summary>
    public class SearchTagsQueryValidator : Validator<SearchTagsQuery>
    {
        public SearchTagsQueryValidator()
        {
            AddRule(x => x.PageNumber.HasValue && x.PageNumber < 0, "Page number cannot be below 0");

            AddRule(x => x.PageSize.HasValue && x.PageSize < 0, "Page size cannot be below 0");

            AddRule(x => x.PageSize.HasValue && x.PageSize > 100, "Page size cannot be above 100");
        }
    }
}
