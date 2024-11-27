namespace Markdown.Markdown.Processing;

public static class MarkdownConstants
{
    public static class Symbols
    {
        public const string SingleEmphasis = "_";
        public const string DoubleEmphasis = "__";
        public const string Header = "#";
    }

    public static class ErrorMessages
    {
        public const string TokensNull = "Token collection cannot be null.";
        public const string ContextNull = "Processing context cannot be null.";
        public const string TokenNull = "Token cannot be null.";
    }
}