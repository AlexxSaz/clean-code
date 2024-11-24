using Markdown.Tokens;

namespace Markdown.Renderers;
public interface IRenderer
{
    string Render(IEnumerable<Token> document);
}