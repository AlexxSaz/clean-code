using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class DifferentWordsEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        if (tokens.Count < 1) return tokens;

        var handledTokens = new List<IToken>();
        var openTagIndices = new Stack<int>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (!IsEmphasisTag(token))
            {
                handledTokens.Add(token);
                continue;
            }
            if (token.IsCloseTag)
            {
                if (openTagIndices.Count <= 0)
                {
                    handledTokens.Add(token);
                    continue;
                }
                var openTagIndex = openTagIndices.Pop();

                var surroundedByTextAndSpace = IsSurroundedByTextAndSpace(tokens, openTagIndex, i);

                if (!surroundedByTextAndSpace)
                {
                    handledTokens.Add(token);
                    continue;
                }
                handledTokens[openTagIndex] = MarkdownTokenCreator.CreateTextToken(tokens[openTagIndex].Content);
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(tokens[i].Content));
                continue;
            }

            openTagIndices.Push(i);
            handledTokens.Add(token);
        }

        return handledTokens;
    }

    private static bool IsSurroundedByTextAndSpace(IReadOnlyList<IToken> tokens, int openTagIndex, int closeTagIndex)
    {
        if (openTagIndex <= 0 || closeTagIndex >= tokens.Count - 1 || openTagIndex >= closeTagIndex) return false;

        return tokens[openTagIndex - 1].Type == TokenType.Text &&
               tokens[closeTagIndex + 1].Type == TokenType.Text &&
               HasSpaceBetween(tokens, openTagIndex, closeTagIndex);
    }

    private static bool HasSpaceBetween(IReadOnlyList<IToken> tokens, int startIndex, int endIndex)
    {
        for (var i = startIndex + 1; i < endIndex; i++)
            if (tokens[i].Type == TokenType.Space)
                return true;

        return false;
    }
}