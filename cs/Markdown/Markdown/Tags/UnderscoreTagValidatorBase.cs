namespace Markdown.Markdown.Tags;

public abstract class UnderscoreTagValidatorBase(string underscoreContent) : ITagValidator
{
    private const char startUnderscoreChar = '_';
    protected readonly string UnderscoreContent = underscoreContent;

    public abstract bool IsValidTag(string content);

    public bool IsTagStart(string content) =>
        content.StartsWith(startUnderscoreChar);
}