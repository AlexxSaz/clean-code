using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class NestedEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledTokens = new List<IToken>();
        var tagStack = new Stack<IToken>();
        foreach (var token in tokens)
        {
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

            if (tagStack.Count > 0 && tagStack.Peek().TagType is TagType.Italic &&
                token.TagType is TagType.Strong)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(token.Content));
                continue;
            }

            tagStack.Push(token);
            handledTokens.Add(token);
        }

        return handledTokens;
    }
}