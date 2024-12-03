using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

internal class HeaderHandler : ITokenHandler
{
    public List<IToken> Handle(IList<IToken> tokens)
    {
        IToken? previousToken = null;
        var handledTokens = new List<IToken>();
        using var enumerator = tokens.GetEnumerator();
        enumerator.MoveNext();

        while (enumerator.Current != null)
        {
            var currentToken = enumerator.Current;

            if ((previousToken == null || previousToken.Type is TokenType.NewLine) &&
                currentToken.Type is TokenType.Header)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateTag("#", TagType.Header));
                enumerator.MoveNext();
                while (!(currentToken.Type is TokenType.NewLine))
                {
                    currentToken = enumerator.Current;
                    handledTokens.Add(currentToken);
                    enumerator.MoveNext();
                }

                handledTokens.Add(MarkdownTokenCreator.CreateCloseTag(currentToken, TagType.Header));
            }

            handledTokens.Add(currentToken);
            previousToken = currentToken;
            enumerator.MoveNext();
        }

        return handledTokens;
    }
}