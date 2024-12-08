using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class NonPairEmphasisHandler : EmphasisHandlerBase
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
            
            if (IsEmphasisTag(token))
            {
                indexTagStack.Push(i);
                tagStack.Push(token);
            }
        }

        while (tagStack.Count > 0)
        {
            var tag = tagStack.Pop();
            var tagIndex = indexTagStack.Pop();

            tokens[tagIndex] = MarkdownTokenCreator.CreateTextToken(tag.Content);
        }

        return tokens;
    }
}