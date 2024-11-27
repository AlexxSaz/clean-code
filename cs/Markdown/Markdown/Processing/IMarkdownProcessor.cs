using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Processing;

public interface IMarkdownProcessor
{
    IList<MarkdownToken> Process(IList<MarkdownToken> tokens);
}