namespace Markdown.Markdown.Tags;

public abstract class UnderscoreTagValidatorBase(string tagMarker) : ITagValidator 
{ 
    private const char UnderscoreChar = '_'; 
    private readonly int tagLength = tagMarker.Length; 
 
    public bool IsValidTag(string content) => 
        HasValidLength(content) && 
        HasValidTagMarkers(content); 
 
    public bool IsTagStart(string content) => 
        !string.IsNullOrEmpty(content) && content.StartsWith(UnderscoreChar); 
 
    private bool HasValidLength(string content) => 
        content.Length == tagLength; 
 
    private bool HasValidTagMarkers(string content) => 
        content.StartsWith(tagMarker) && content.EndsWith(tagMarker); 
}