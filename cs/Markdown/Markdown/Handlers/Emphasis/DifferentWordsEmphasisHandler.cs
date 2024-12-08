using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class DifferentWordsEmphasisHandler : EmphasisHandlerBase
{
    public override IList<IToken> Handle(IList<IToken> tokens)
    {
        if (tokens.Count < 1) return tokens;

        var openTagIndices = new Stack<int>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (!IsEmphasisTag(token)) continue;
            if (token.IsCloseTag)
            {
                if (openTagIndices.Count <= 0) continue;
                var openTagIndex = openTagIndices.Pop();

                var surroundedByTextAndSpace = IsSurroundedByTextAndSpace(tokens, openTagIndex, i);

                if (!surroundedByTextAndSpace) continue;
                tokens[openTagIndex] = MarkdownTokenCreator.CreateTextToken(tokens[openTagIndex].Content);
                tokens[i] = MarkdownTokenCreator.CreateTextToken(tokens[i].Content);
            }
            else
            {
                openTagIndices.Push(i);
            }
        }

        return tokens;
    }

    private static bool IsSurroundedByTextAndSpace(IList<IToken> tokens, int openTagIndex, int closeTagIndex)
    {
        if (openTagIndex <= 0 || closeTagIndex >= tokens.Count - 1 || openTagIndex >= closeTagIndex) return false;

        return tokens[openTagIndex - 1].Type == TokenType.Text &&
               tokens[closeTagIndex + 1].Type == TokenType.Text &&
               HasSpaceBetween(tokens, openTagIndex, closeTagIndex);
    }

    private static bool HasSpaceBetween(IList<IToken> tokens, int startIndex, int endIndex)
    {
        for (var i = startIndex + 1; i < endIndex; i++)
            if (tokens[i].Type == TokenType.Space)
                return true;

        return false;
    }
}