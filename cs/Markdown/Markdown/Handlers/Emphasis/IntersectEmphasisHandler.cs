using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class IntersectEmphasisHandler : EmphasisHandlerBase
{
    public override List<IToken> Handle(IList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var handledStack = new Stack<IToken>();
        var isPossibleIntersect = false;
        var skipNextTag = false;
        foreach (var token in tokens)
        {
            if (tagStack.Count > 0 && IsEmphasisTag(token))
            {
                if (!tagStack.Peek().Equals(token) && !isPossibleIntersect)
                {
                    isPossibleIntersect = true;
                }
                else if (!tagStack.Peek().Equals(token) && isPossibleIntersect)
                {
                    skipNextTag = true;
                    while (tagStack.Count > 0)
                    {
                        var tag = tagStack.Pop();
                        var bufferStack = new Stack<IToken>();
                        while (handledStack.Count > 0 && !handledStack.Peek().Equals(tag))
                        {
                            bufferStack.Push(handledStack.Pop());
                        }

                        handledStack.Push(MarkdownTokenCreator.CreateTextToken(handledStack.Pop().Content));
                        while (bufferStack.Count > 0)
                        {
                            handledStack.Push(bufferStack.Pop());
                        }
                    }
                }
                else
                {
                    tagStack.Pop();
                    handledStack.Push(token);
                    continue;
                }
            }

            if (IsEmphasisTag(token) && !skipNextTag)
                tagStack.Push(token);
            else if (IsEmphasisTag(token) && skipNextTag)
            {
                skipNextTag = false;
                handledStack.Push(MarkdownTokenCreator.CreateTextToken(token.Content));
                continue;
            }

            handledStack.Push(token);
        }

        return handledStack.Reverse().ToList();
    }
}