using Markdown.Markdown;
using Markdown.Markdown.Processing;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    private readonly IRenderer _renderer = new HtmlRenderer();
    private readonly MarkdownTokenParser _tokenParser = new();
    private readonly MarkdownProcessor _markdownProcessor = new();

    public string Render(string text)
    {
        var tokens = _tokenParser
            .Parse(text)
            .ToArray();
        var processedTokens = _markdownProcessor.Process(tokens);
        return _renderer.Render(processedTokens);
    }
}