using Markdown.Markdown;
using Markdown.Markdown.Handlers;
using Markdown.Markdown.Tokens;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    private readonly IRenderer _renderer = new HtmlRenderer();
    private readonly TextTokenizer _textTokenizer = new();

    private readonly IEnumerable<ITokenHandler> _handlers =
        TokenHandlerFactory.CreateHandlers();

    public string Render(string text)
    {
        var tokens = _textTokenizer.Tokenize(text).ToList();
        foreach (var handler in _handlers)
            tokens = handler.Handle(tokens);
        return _renderer.Render(tokens);
    }
}