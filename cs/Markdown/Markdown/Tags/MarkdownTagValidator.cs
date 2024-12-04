namespace Markdown.Markdown.Tags;

public static class MarkdownTagValidator
{
    private static readonly ITagValidator[] tagValidators = 
    [
        new StrongTagValidator(),
        new ItalicTagValidator(),
        new HeaderTagValidator()
    ];

    public static bool Validate(string content) =>
        tagValidators.Any(validator => validator.IsValidTag(content));

    public static bool IsTagStart(string content) =>
        tagValidators.Any(validator => validator.IsTagStart(content));
}