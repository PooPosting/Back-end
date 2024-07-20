using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PooPosting.Domain.DbContext.Interfaces;
using PooPosting.Domain.Exceptions;

namespace PooPosting.Domain.DbContext.Pagination;

public static class DbSetPagination
{
    // todo: come up with auto ordering by ID. It would probably involve paging the DB entities and then mapping them.
    // maybe .PaginateAsQuery() (totalItems would be queryable, then I can somehow project it later?)
    public static async Task<PagedResult<T>> Paginate<T>(
        this IQueryable<T> query,
        IQueryParams queryParams
    ) where T : class
    {
        if (queryParams.OrderBy is not null)
        {
            query = query.OrderByDynamic(queryParams);
        }
        
        var items = await query
            .Skip(queryParams.PageSize * (queryParams.PageNumber - 1))
            .Take(queryParams.PageSize)
            .ToListAsync();
        var totalItems = await query.CountAsync();
        
        return new PagedResult<T>(
            items,
            queryParams.PageNumber,
            queryParams.PageSize,
            totalItems
        );
    }
    
    private static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, IQueryParams queryParams)
    {
        var command = queryParams.Direction == "desc" ? "OrderByDescending" : "OrderBy";
        var type = typeof(T);
        var property = type.GetProperties().FirstOrDefault(p => p.Name.Equals(queryParams.OrderBy, StringComparison.OrdinalIgnoreCase));
        if (property == null) throw new BadRequestException($"Property '{queryParams.OrderBy}' does not exist on type '{type.Name}'");

        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, property.PropertyType },
            source.Expression, Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<T>(resultExpression);
    }
}