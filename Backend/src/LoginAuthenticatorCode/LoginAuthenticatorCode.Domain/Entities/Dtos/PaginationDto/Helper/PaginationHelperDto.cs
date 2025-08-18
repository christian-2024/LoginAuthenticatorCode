using Microsoft.EntityFrameworkCore;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto.Helper;

public class PaginationHelperDto
{
    public static async Task<PagedListDto<T>> CreateAsync<T>(IQueryable<T> source, int pageNumber, int pageSize) where T : class
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        return new PagedListDto<T>(items, count, pageNumber, pageSize);
    }
}

