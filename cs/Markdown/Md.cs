using Markdown.Renderers;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    private readonly IRenderer _renderer = new HtmlRenderer();
    private readonly MarkdownTokenParser _tokenParser = new();

    public string Render(string text)
    {
        var tokens = _tokenParser.Parse(text);
        return _renderer.Render(tokens);
    }
}