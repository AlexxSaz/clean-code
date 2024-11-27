using Markdown.Markdown.Tokens;

namespace Markdown.Renderers;
public interface IRenderer
{
    string Render(IList<MarkdownToken> document);
}