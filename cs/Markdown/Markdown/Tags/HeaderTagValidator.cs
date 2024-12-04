namespace Markdown.Markdown.Tags;

public class HeaderTagValidator : ITagValidator
{
    private const char startTagContent = '#'; 
    private const char endTagContent = ' '; 

    public bool IsValidTag(string content) =>
        content is [startTagContent, endTagContent];

    public bool IsTagStart(string content) =>
        content.StartsWith(startTagContent);
}