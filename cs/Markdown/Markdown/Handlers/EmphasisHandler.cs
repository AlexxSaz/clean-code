using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class EmphasisHandler : ITokenHandler
{
    private static readonly IToken UnderscoreToken = new MarkdownToken("_", TokenType.Tag);
    private static readonly IToken DoubleUnderscoreToken = new MarkdownToken("__", TokenType.Tag);

    public List<IToken> Handle(IList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var handledStack = new Stack<IToken>();
        foreach (var token in tokens)
        {
            if (tagStack.Count > 0 && tagStack.Peek().Equals(token))
            {
                var tagType = token.Content == "_" ? TagType.Italic : TagType.Strong;
                var firstTag = tagStack.Pop();
                var bufferStack = new Stack<IToken>();
                var isHasText = false;

                while (handledStack.Peek() != firstTag)
                {
                    var lastHandled = handledStack.Pop();
                    if (lastHandled.Type is TokenType.Text)
                        isHasText = true;
                    bufferStack.Push(lastHandled);
                }
                handledStack.Pop();

                var openTag = isHasText ? MarkdownTokenCreator.CreateOpenTag(firstTag, tagType) : firstTag;
                var closeTag = isHasText ? MarkdownTokenCreator.CreateCloseTag(firstTag, tagType) : firstTag;
                handledStack.Push(openTag);
                while (bufferStack.Count > 0)
                    handledStack.Push(bufferStack.Pop());
                handledStack.Push(closeTag);
                
                continue;
            }

            if (token.Equals(UnderscoreToken))
            {
                tagStack.Push(token);
            }

            if (token.Equals(DoubleUnderscoreToken))
            {
                tagStack.Push(token);
            }

            handledStack.Push(token);
        }

        return handledStack.Reverse().ToList();
    }
}