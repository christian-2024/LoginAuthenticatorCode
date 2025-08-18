using System.Diagnostics.CodeAnalysis;

namespace LoginAuthenticatorCode.Shared.Extension.Helper;

public static class ObjectExtension
{
    public static bool IsNull([NotNullWhen(false)] this object? source) => source is null;

    public static bool HasValue([NotNullWhen(true)] this object? source) => source is not null;
}