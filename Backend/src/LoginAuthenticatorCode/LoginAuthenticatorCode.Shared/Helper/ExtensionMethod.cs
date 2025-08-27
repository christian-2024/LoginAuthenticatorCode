namespace LoginAuthenticatorCode.Shared.Helper;

public static class ExtensionMethod
{
    public static string RemoveScore(this string text)
    {
        return !string.IsNullOrEmpty(text) ? text.Replace("-", "").Replace(".", "").Replace("/", "").Replace("(", "").Replace(")", "").Replace(" ", "") : text;
    }

    public static string RemoveSpace(this string text)
    {
        return !string.IsNullOrEmpty(text) ? text.ToLower().Trim().TrimStart().TrimEnd() : text;
    }
}