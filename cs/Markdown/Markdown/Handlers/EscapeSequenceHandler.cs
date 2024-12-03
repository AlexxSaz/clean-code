using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class EscapeSequenceHandler : ITokenHandler
{
    public List<IToken> Handle(IList<IToken> tokens)
    {
        IToken previousToken = null;
        var handledTokens = new List<IToken>();
        foreach(var token in tokens)
        {
            if (previousToken is { Type: TokenType.Escape } &&
                token?.Type is TokenType.Tag or TokenType.Escape)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(token.Content));
            }
            else if (token?.Type is TokenType.Escape)
            {
                previousToken = token;
            }
            else
            {
                handledTokens.Add(token);
                previousToken = token;
            }
        }
        return handledTokens;
    }
}