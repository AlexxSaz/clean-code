using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Processing;

public class TokenProcessingContextModifier(IProcessingContext processingContext)
{
    public void AddTokenToResult(MarkdownToken? token)
    {
        if (token == null)
            throw new ArgumentNullException(nameof(token), 
                MarkdownConstants
                    .ErrorMessages
                    .TokenNull);
        processingContext.Result.Add(token);
    }

    public void AddTokensToResult(IEnumerable<MarkdownToken>? tokens)
    {
        if (tokens == null)
            throw new ArgumentNullException(nameof(tokens), 
                MarkdownConstants
                    .ErrorMessages
                    .TokensNull);
        foreach (var token in tokens)
            processingContext.Result.Add(token);
    }
}