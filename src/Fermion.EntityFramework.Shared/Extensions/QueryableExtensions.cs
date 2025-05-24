using System.Linq.Dynamic.Core;
using Fermion.EntityFramework.Shared.DTOs.Pagination;
using Fermion.EntityFramework.Shared.DTOs.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Fermion.EntityFramework.Shared.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> queryable,
        IEnumerable<SortRequestDto>? sorts,
        CancellationToken cancellationToken = default)
    {
        if (sorts == null)
        {
            return queryable;
        }

        var sortRequests = sorts.ToList();
        if (sortRequests.Count == 0)
        {
            return queryable;
        }

        sortRequests = sortRequests.Where(x => !string.IsNullOrWhiteSpace(x.Field)).ToList();
        if (sortRequests.Count == 0)
        {
            return queryable;
        }

        var sortString = string.Join(",", sortRequests.Select(x => $"{x.Field} {x.Order}"));
        return queryable.OrderBy(sortString, cancellationToken);
    }

    public static async Task<PageableResourceDto<T>> ToPageableAsync<T>(
        this IQueryable<T> queryable,
        int page,
        int perPage,
        CancellationToken cancellationToken = default
    )
    {
        var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

        if (count == 0)
        {
            return new PageableResourceDto<T>([], count, page, perPage);
        }

        var items = await queryable
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var list = new PageableResourceDto<T>(items, count, page, perPage);

        return list;
    }
}