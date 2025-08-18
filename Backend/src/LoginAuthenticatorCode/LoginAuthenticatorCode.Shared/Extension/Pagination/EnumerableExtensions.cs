using LoginAuthenticatorCode.Shared.Extension.Helper;
using System.Diagnostics.CodeAnalysis;

namespace LoginAuthenticatorCode.Shared.Extension.Pagination;

public static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();

    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source) => source.IsNull() || !source.Any();

    public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        var skip = pageSize * (page - 1);
        var take = pageSize;

        return source
            .Skip(skip)
            .Take(take);
    }
}