using Markdown.Html.Tags;

namespace Markdown.Html;

public class HtmlTagConverter
{
    private readonly IHtmlTag[] _htmlTags =
    [
        new HeaderTag(),
        new BoldTag(),
        new ItalicTag()
    ];

    public string Convert(string tagContent, bool isClosedTag)
    {
        foreach (var htmlTag in _htmlTags)
        {
            var isConvertedToHtml = htmlTag.TryConvert(tagContent, isClosedTag, out var resultHtmlTag);

            if (isConvertedToHtml)
                return resultHtmlTag;
        }

        throw new ArgumentException($"{nameof(tagContent)} doesn't match to any html tag.");
    }
}