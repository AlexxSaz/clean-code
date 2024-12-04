namespace Markdown.Markdown.Tags;

public abstract class UnderscoreTagValidatorBase(string tagMarker) : ITagValidator
{
    private const char UnderscoreChar = '_';
    private readonly int tagLength = tagMarker.Length;

    public bool IsValidTag(string content)
    {
        return HasValidLength(content) &&
               HasValidTagMarkers(content) &&
               HasValidBoundaries(content);
    }

    public bool IsTagStart(string content) =>
        !string.IsNullOrEmpty(content) && content.StartsWith(UnderscoreChar);

    private bool HasValidLength(string content)
    {
        var minimumLength = tagLength;
        return content.Length >= minimumLength;
    }

    private bool HasValidTagMarkers(string content)
    {
        return content.StartsWith(tagMarker) && content.EndsWith(tagMarker);
    }

    private bool HasValidBoundaries(string content)
    {
        var firstContentChar = content[tagLength - 1];
        if (char.IsWhiteSpace(firstContentChar))
            return false;

        var lastContentChar = content[^(tagLength + 1)];
        if (char.IsWhiteSpace(lastContentChar))
            return false;

        return true;
    }
}