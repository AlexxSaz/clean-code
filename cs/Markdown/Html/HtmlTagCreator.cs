using Markdown.Html.Tags;

namespace Markdown.Html;

public static class HtmlTagCreator
{
    private static readonly Dictionary<TagType, string> tagToString = new()
    {
        { TagType.Header, "h1" },
        { TagType.Italic, "em" },
        { TagType.Strong, "strong" }
    };
    
    public static string GetStringContent(TagType tagType) =>
        tagToString[tagType];
}