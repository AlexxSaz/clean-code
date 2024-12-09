using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class IntersectEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledTokens = new List<IToken>();
        var tagStack = new Stack<IToken>();
        var indexTagStack = new Stack<int>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                tagStack.Pop();
                handledTokens.Add(token);
                continue;
            }

            if (!IsEmphasisTag(token))
            {
                handledTokens.Add(token);
                continue;
            }

            if (token.IsCloseTag)
            {
                while (tagStack.Count > 0)
                {
                    var idx = indexTagStack.Pop();
                    var tag = tagStack.Pop();
                    handledTokens[idx] = MarkdownTokenCreator.CreateTextToken(tag.Content);
                }

                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(token.Content));
                continue;
            }

            indexTagStack.Push(i);
            tagStack.Push(token);
            handledTokens.Add(token);
        }

        return handledTokens;
    }
}