using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

internal class HeaderHandler : ITokenHandler
{
    public List<IToken> Handle(IList<IToken> tokens)
    {
        IToken? previousToken = null;
        var handledTokens = new List<IToken>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];

            if ((previousToken == null || previousToken.Type is TokenType.NewLine) &&
                currentToken.Type is TokenType.Tag && currentToken.TagType is TagType.None)
            {
                var openHeaderTag = MarkdownTokenCreator.CreateOpenTag(currentToken, TagType.Header);
                handledTokens.Add(openHeaderTag);
                i++;                
                while (i < tokens.Count && currentToken.Type is not TokenType.NewLine)
                {
                    currentToken = tokens[i];
                    handledTokens.Add(currentToken);
                    i++;
                }

                handledTokens.Add(MarkdownTokenCreator.CreateCloseTag(openHeaderTag, TagType.Header));
            }
            else handledTokens.Add(currentToken);
            previousToken = currentToken;
        }

        return handledTokens;
    }
}