using Markdown.Markdown.Handlers;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Processing;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly IEnumerable<ITokenHandler> _tokenHandlers =
        TokenHandlerFactory.CreateHandlers();

    public IList<MarkdownToken> Process(IList<MarkdownToken> tokens)
    {
        var tokenContext = new TokenProcessingContext(tokens);
        var resultContextModifier = new TokenProcessingContextModifier(tokenContext);

        while (tokenContext.HasMoreTokens)
        {
            foreach (var handler in _tokenHandlers)
            {
                if (handler.CanHandle(tokenContext.CurrentToken) &&
                    handler.Handle(tokenContext))
                    break;

                resultContextModifier.AddTokenToResult(tokenContext.CurrentToken);
                tokenContext.CurrentIndex++;
            }
        }

        return tokenContext.Result;
    }
}