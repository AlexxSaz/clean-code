namespace Markdown.Html.Tags;

public interface IHtmlTag
{
    bool TryConvert(string content, bool isClosedTag, out string resultContent);
}