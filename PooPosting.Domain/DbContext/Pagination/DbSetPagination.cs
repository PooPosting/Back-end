using Microsoft.EntityFrameworkCore;
using PooPosting.Domain.DbContext.Interfaces;

namespace PooPosting.Domain.DbContext.Pagination;

public static class DbSetPagination
{
    // todo: come up with auto ordering by ID. It would probably involve paging the DB entities and then mapping them.
    // maybe .PaginateAsQuery() (totalItems would be queryable, then I can somehow project it later?)
    public static async Task<PagedResult<T>> Paginate<T>(
        this IQueryable<T> query,
        IPaginationParameters pagination
    ) where T : class
    {
        var items = await query
            .Skip(pagination.PageSize * (pagination.PageNumber - 1))
            .Take(pagination.PageSize)
            .ToListAsync();
        var totalItems = await query.CountAsync();
        
        return new PagedResult<T>(
            items,
            pagination.PageNumber,
            pagination.PageSize,
            totalItems
        );
    }
}