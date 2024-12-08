using Markdown.Markdown;
using Markdown.Markdown.Handlers;
using Markdown.Markdown.Tokens;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    private readonly IRenderer _renderer = new HtmlRenderer();
    private readonly TextTokenizer _textTokenizer = new();

    public string Render(string text)
    {
        var tokens = _textTokenizer.Tokenize(text);
        return _renderer.Render(tokens.ToList());
    }
}