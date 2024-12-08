using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class DifferentWordsEmphasisHandler : EmphasisHandlerBase
{
    public override IList<IToken> Handle(IList<IToken> tokens)
    {
        if (tokens.Count < 1)
            return tokens;

        var openTagIndex = -1;

        IToken? previousToken = null;
        var token = tokens[0];
        for (var i = 1; i < tokens.Count; i++)
        {
            var nextToken = tokens[i];

            if (IsEmphasisTag(token) && token.IsCloseTag)
            {
                var isOpenTagInsideText = openTagIndex > 0 && openTagIndex < tokens.Count - 1 &&
                                          tokens[openTagIndex - 1].Type is TokenType.Text &&
                                          tokens[openTagIndex + 1].Type is TokenType.Text;
                var isCloseTagInsideText = previousToken is { Type: TokenType.Text } &&
                                           nextToken.Type is TokenType.Text;
                var isDifferentWord = false;
                var j = i - 1;
                while ((j > -1 && j != openTagIndex) || isDifferentWord)
                {
                    if (tokens[j].Type is TokenType.Space)
                    {
                        isDifferentWord = true;
                        break;
                    }

                    j--;
                }

                if (isDifferentWord && isCloseTagInsideText && isOpenTagInsideText)
                {
                    tokens[openTagIndex] = MarkdownTokenCreator.CreateTextToken(tokens[openTagIndex].Content);
                    tokens[i - 1] = MarkdownTokenCreator.CreateTextToken(tokens[i - 1].Content);
                    openTagIndex = -1;
                }
            }


            if (IsEmphasisTag(token) && !token.IsCloseTag)
            {
                openTagIndex = i - 1;
            }

            previousToken = token;
            token = nextToken;
        }

        return tokens;
    }
}