using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class EmphasisHandler : EmphasisHandlerBase
{
    private readonly EmphasisHandlerBase[] emphasisHandlers =
    [
        new PairEmphasisHandler(),
        new SkipEmphasisHandler(),
        new IntersectEmphasisHandler(),
        new NestedEmphasisHandler(),
        new NonPairEmphasisHandler(),
        new DifferentWordsEmphasisHandler()
    ];

    public override List<IToken> Handle(IList<IToken> tokens)
    {
        tokens = emphasisHandlers.Aggregate(tokens, (current, emphasisHandler) => emphasisHandler.Handle(current));
        
        var tagStack = new Stack<IToken>();
        var handledStack = new Stack<IToken>();

        foreach (var token in tokens)
        {
            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                HandleClosingEmphasis(token, tagStack, handledStack);
                continue;
            }

            if (IsEmphasisTag(token))
                tagStack.Push(token);

            handledStack.Push(token);
        }

        return handledStack.Reverse().ToList();
    }

    private static void HandleClosingEmphasis(IToken token, Stack<IToken> tagStack, Stack<IToken> handledStack)
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
    }
}