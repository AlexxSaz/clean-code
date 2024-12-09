using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class SkipEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        if (tokens.Count < 1)
            return tokens;

        var handledTokens = new List<IToken>();

        IToken? previousToken = null;
        var token = tokens[0];
        for (var i = 1; i < tokens.Count; i++)
        {
            var nextToken = tokens[i];

            if (IsEmphasisTag(token) && (!token.IsCloseTag && nextToken.Type is TokenType.Space ||
                                         previousToken is { Type: TokenType.Space } && token.IsCloseTag))
            {
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(token.Content));
                previousToken = token;
                token = nextToken;
                continue;
            }

            handledTokens.Add(token);
            previousToken = token;
            token = nextToken;
        }

        handledTokens.Add(token);

        return handledTokens;
    }
}