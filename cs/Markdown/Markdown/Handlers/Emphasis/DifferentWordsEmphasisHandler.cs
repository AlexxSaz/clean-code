using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class DifferentWordsEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        if (tokens.Count < 1) return tokens;

        var handledTokens = tokens;

        for (var i = tokens.Count - 1; i > -1; i--)
        {
            var token = tokens[i];
            if (!token.IsCloseTag) continue;

            var openTagIndex = GetOpenTagIndex(tokens, token.TagPair!);
            if (openTagIndex == -1) continue;

            if (IsSurroundedByTextAndSpace(tokens, openTagIndex, i))
                handledTokens = RewriteTagsToText(handledTokens, openTagIndex, i);
        }

        return handledTokens;
    }

    private static int GetOpenTagIndex(IReadOnlyList<IToken> tokens, IToken searchToken)
    {
        for (var i = 0; i < tokens.Count; i++)
            if (tokens[i] == searchToken)
                return i;
        return -1;
    }

    private static List<IToken> RewriteTagsToText(IReadOnlyList<IToken> tokens, int openTagIndex, int closeTagIndex)
    {
        var handledTokens = new List<IToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (i == openTagIndex || i == closeTagIndex)
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(token.Content));
            else
                handledTokens.Add(tokens[i]);
        }

        return handledTokens;
    }

    private static bool IsSurroundedByTextAndSpace(IReadOnlyList<IToken> tokens, int openTagIndex, int closeTagIndex)
    {
        if (openTagIndex <= 0 || closeTagIndex >= tokens.Count - 1 || openTagIndex >= closeTagIndex) return false;

        return tokens[openTagIndex - 1].Type == TokenType.Text &&
               tokens[openTagIndex + 1].Type == TokenType.Text &&
               tokens[closeTagIndex - 1].Type == TokenType.Text &&
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