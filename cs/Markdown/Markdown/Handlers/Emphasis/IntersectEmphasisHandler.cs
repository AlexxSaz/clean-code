using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class IntersectEmphasisHandler : EmphasisHandlerBase
{
    public override IList<IToken> Handle(IList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var indexTagStack = new Stack<int>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                tagStack.Pop();
                continue;
            }

            if (!IsEmphasisTag(token)) continue;
            if (token.IsCloseTag)
            {
                while (tagStack.Count > 0)
                {
                    var idx = indexTagStack.Pop();
                    var tag = tagStack.Pop();
                    tokens[idx] = MarkdownTokenCreator.CreateTextToken(tag.Content);
                }

                tokens[i] = MarkdownTokenCreator.CreateTextToken(token.Content);
            }
            else
            {
                indexTagStack.Push(i);
                tagStack.Push(token);
            }
        }

        return tokens;
    }
}