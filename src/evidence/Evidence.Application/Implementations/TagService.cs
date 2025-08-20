using Evidence.Application.Codes;
using Evidence.Core.Models;
using Evidence.Core.Services;
using Evidence.Core.ValueObjects;
using Evidence.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Evidence.Application.Implementations
{
    internal class TagService(EvidenceApplicationDbContext dbContext) : ITagService
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;

        public async Task<IResult> CreateAsync(Tag tag)
        {
            var result = new Result();

            var isNameTaken = await _dbcontext.Tags.AnyAsync(x => x.Name == tag.Name);
            if (isNameTaken)
            {
                result.AddError(BusinessRuleCodes.TAG_NAME_USED, "Tag name already used");
                return result;
            }

            await _dbcontext.Tags.AddAsync(tag);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<PaginatedResult<Tag>> SearchAsync(SearchTagsQuery query)
        {
            var queryBuilder = _dbcontext.Tags.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                queryBuilder = queryBuilder.Where(x => x.Name.Contains(query.Name));
            }

            var pageSize = query.PageSize ?? 10;
            var pageNumber = query.PageNumber ?? 1;

            var totalCount = await queryBuilder.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pageItems = await queryBuilder
             .OrderBy(x => x.Name)
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync();

            var paginationMetaData = new PaginationMetadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalCount
            };

            return new PaginatedResult<Tag> { Data = pageItems, Pagination = paginationMetaData, HasNextPage = pageNumber < totalPages, HasPreviousPage = pageNumber > 1};
        }
    }
}
