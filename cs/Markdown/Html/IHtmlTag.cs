using Markdown.Tokens;

namespace Markdown.Html;

public interface IHtmlTag
{
    Token ToHtml(string value);
}