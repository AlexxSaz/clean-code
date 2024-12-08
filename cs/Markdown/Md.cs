using Markdown.Markdown;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    private readonly IRenderer renderer = new HtmlRenderer();
    private readonly ITokenizer textTokenizer = new TextTokenizer();

    public string Render(string text)
    {
        var tokens = textTokenizer.Tokenize(text);
        return renderer.Render(tokens);
    }
}