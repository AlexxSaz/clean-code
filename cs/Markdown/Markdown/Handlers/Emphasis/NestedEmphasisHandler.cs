using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class NestedEmphasisHandler : EmphasisHandlerBase
{
    public override IList<IToken> Handle(IList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                tagStack.Pop();
                continue;
            }

            if (!IsEmphasisTag(token)) continue;
            if (tagStack.Count > 0 && tagStack.Peek().TagType is TagType.Italic &&
                token.TagType is TagType.Strong)
                tokens[i] = MarkdownTokenCreator.CreateTextToken(token.Content);
            else
                tagStack.Push(token);
        }

        return tokens;
    }
}