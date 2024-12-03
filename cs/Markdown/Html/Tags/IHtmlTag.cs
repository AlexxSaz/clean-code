using Markdown.Markdown.Tokens;

namespace Markdown.Html.Tags;

public interface IHtmlTag
{
    bool TryConvert(IToken token, out string resultContent);
}