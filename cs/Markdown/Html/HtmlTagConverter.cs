using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Html;

public class HtmlTagConverter
{
    private readonly ITagConverter[] _htmlTags =
    [
        new HeaderTagConverter(),
        new StrongTagConverter(),
        new ItalicTagConverter()
    ];

    public string Convert(IToken token)
    {
        foreach (var htmlTag in _htmlTags)
        {
            var isConvertedToHtml = htmlTag.TryConvert(token, out var resultHtmlTag);

            if (isConvertedToHtml)
                return resultHtmlTag;
        }

        throw new ArgumentException($"{nameof(token.Content)} doesn't match to any html tag.");
    }
}