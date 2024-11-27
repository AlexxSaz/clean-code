using Markdown.Markdown.Processing;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public interface ITokenHandler
{
    bool CanHandle(MarkdownToken? token);
    bool Handle(IProcessingContext context);
}