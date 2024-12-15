using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

internal class HeaderHandler : ITokenHandler
{
    public IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        IToken? previousToken = null;
        var handledTokens = new List<IToken>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];

            if (previousToken != null &&
                currentToken.Type is TokenType.Tag && currentToken.TagType is TagType.Header)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(currentToken.Content));
                continue;
            }
            
            if (previousToken == null &&
                currentToken.Type is TokenType.Tag && currentToken.TagType is TagType.Header)
            {
                var openHeaderTag = currentToken;
                while (i < tokens.Count - 1 && currentToken.Type is not TokenType.NewLine)
                {
                    handledTokens.Add(currentToken);
                    currentToken = tokens[++i];
                }

                handledTokens.Add(currentToken);
                handledTokens.Add(
                    MarkdownTokenCreator.CreateCloseTag(openHeaderTag, openHeaderTag.TagType, openHeaderTag));
            }
            else handledTokens.Add(currentToken);

            previousToken = currentToken;
        }

        return handledTokens;
    }
}