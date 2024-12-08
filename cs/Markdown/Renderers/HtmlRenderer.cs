using System.Text;
using Markdown.Html;
using Markdown.Markdown;
using Markdown.Markdown.Tokens;

namespace Markdown.Renderers;

public class HtmlRenderer : IRenderer
{
    private readonly HtmlTagConverter tagConverter = new();

    public string Render(IList<IToken> tokens)
    {
        var tokenBuilder = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token?.Type is TokenType.Tag)
                tokenBuilder.Append(tagConverter.Convert(token));
            else
                tokenBuilder.Append(token?.Content);
        }

        return tokenBuilder.ToString();
    }
}