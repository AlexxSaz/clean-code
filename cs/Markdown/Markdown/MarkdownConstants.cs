namespace Markdown.Markdown;

internal static class MarkdownConstants
{
    public static class Symbols
    {
        public const char Newline = '\n';
        public const char CarriageReturn = '\r';
        public static readonly HashSet<char> SeparatorSymbols = ['\\', ',', '.', ';', '/'];
    }

    public static class TagSymbols
    {
        public const string SingleUnderscore = "_";
        public const string DoubleUnderscore = "__";
        public const string Header = "#";
    }

    public static class ErrorMessages
    {
        public const string TokensNull = "Token collection cannot be null.";
        public const string ContextNull = "Processing context cannot be null.";
        public const string TokenNull = "Token cannot be null.";
    }
}