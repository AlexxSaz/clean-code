using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class EscapeSequenceHandler : ITokenHandler
{
    public List<IToken> Handle(IList<IToken> tokens)
    {
        IToken? previousToken = null;
        var handledTokens = new List<IToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];
            if (previousToken is { Type: TokenType.Escape } &&
                currentToken.Type is TokenType.Tag or TokenType.Escape)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(currentToken.Content));
                previousToken = null;
            }
            else if (currentToken.Type is TokenType.Escape)
            {
                previousToken = currentToken;
            }
            else
            {
                handledTokens.Add(currentToken);
                previousToken = currentToken;
            }
        }

        return handledTokens;
    }
}