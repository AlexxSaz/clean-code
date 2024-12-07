using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class EmphasisHandler : EmphasisHandlerBase
{
    private readonly EmphasisHandlerBase[] emphasisHandlers =
    [
        new SkipEmphasisHandler(),
        new IntersectEmphasisHandler()
    ];

    public override List<IToken> Handle(IList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var handledStack = new Stack<IToken>();

        foreach (var token in tokens)
        {
            if (tagStack.Count > 0 && tagStack.Peek().Equals(token))
            {
                HandleClosingEmphasis(token, tagStack, handledStack);
                continue;
            }

            if (IsEmphasisTag(token))
                tagStack.Push(token);

            handledStack.Push(token);
        }

        tokens = handledStack.Reverse().ToList();
        tokens = emphasisHandlers.Aggregate(tokens, (current, emphasisHandler) => emphasisHandler.Handle(current));

        return tokens.ToList();
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
            handledStack.Pop(); // Remove opening tag
            var tagType = token.Content == "_" ? TagType.Italic : TagType.Strong;
            var openTag = isHasText ? MarkdownTokenCreator.CreateOpenTag(firstTag, tagType) : firstTag;
            var closeTag = isHasText ? MarkdownTokenCreator.CreateCloseTag(firstTag, tagType) : firstTag;

            handledStack.Push(openTag);
            while (bufferStack.Count > 0)
                handledStack.Push(bufferStack.Pop());
            handledStack.Push(closeTag);
        }
        else
        {
            // Invalid tag structure - restore the buffer and add token as text
            while (bufferStack.Count > 0)
                handledStack.Push(bufferStack.Pop());
            handledStack.Push(MarkdownTokenCreator.CreateTextToken(token.Content));
        }
    }
}