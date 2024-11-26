using Markdown.Markdown;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    private readonly IRenderer _renderer = new HtmlRenderer();
    private readonly MarkdownTokenParser _tokenParser = new();
    private readonly MarkdownTokenCombiner _tokenCombiner = new();

    public string Render(string text)
    {
        var tokens = _tokenParser
            .Parse(text)
            .ToArray();
        var combinedTokens = _tokenCombiner.Combine(tokens);
        return _renderer.Render(combinedTokens);
    }
}