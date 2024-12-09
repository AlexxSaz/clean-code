using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class TextInsideEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var handledStack = new Stack<IToken>();

        foreach (var token in tokens)
        {
            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                var firstTag = tagStack.Pop();
                var bufferStack = new Stack<IToken>();
                var isHasText = false;

                while (handledStack.Count > 0 && handledStack.Peek() != firstTag)
                {
                    var lastHandled = handledStack.Pop();
                    if (lastHandled.Type is TokenType.Text)
                        isHasText = true;
                    bufferStack.Push(lastHandled);
                }

                if (handledStack.Count > 0)
                {
                    handledStack.Pop();
                    var openTag = isHasText ? firstTag : MarkdownTokenCreator.CreateTextToken(firstTag.Content);
                    var closeTag = isHasText ? token : MarkdownTokenCreator.CreateTextToken(token.Content);

                    handledStack.Push(openTag);
                    while (bufferStack.Count > 0)
                        handledStack.Push(bufferStack.Pop());
                    handledStack.Push(closeTag);
                }
                else
                {
                    while (bufferStack.Count > 0)
                        handledStack.Push(bufferStack.Pop());
                    handledStack.Push(MarkdownTokenCreator.CreateTextToken(token.Content));
                }
                continue;
            }

            if (IsEmphasisTag(token))
                tagStack.Push(token);

            handledStack.Push(token);
        }

        return handledStack.Reverse().ToList();
    }
}