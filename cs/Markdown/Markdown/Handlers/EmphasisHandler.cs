using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class EmphasisHandler : ITokenHandler
{
    private static readonly IToken UnderscoreToken = new MarkdownToken("_", TokenType.Tag);
    private static readonly IToken DoubleUnderscoreToken = new MarkdownToken("__", TokenType.Tag);

    public List<IToken> Handle(IList<IToken> tokens)
    {
        if (tokens.Count < 1)
            return tokens.ToList();

        var tagStack = new Stack<IToken>();
        var handledStack = new Stack<IToken>();
        
        IToken? previousToken = null;
        var token = tokens[0];        
        for (var i = 1; i < tokens.Count; i++)
        {
            var nextToken = tokens[i];
            
            if (IsEmphasisToken(token))
            {
                if (ShouldSkipEmphasis(previousToken, nextToken))
                {
                    handledStack.Push(MarkdownTokenCreator.CreateTextToken(token.Content));
                }
                else if (tagStack.Count > 0 && tagStack.Peek().Equals(token))
                {
                    HandleClosingEmphasis(token, tagStack, handledStack);
                }
                else
                {
                    tagStack.Push(token);
                    handledStack.Push(token);
                }
            }
            else
            {
                handledStack.Push(token);
            }
            
            previousToken = token;
            token = nextToken;
        }
        handledStack.Push(tokens.Last());
        
        return handledStack.Reverse().ToList();
    }

    private bool IsEmphasisToken(IToken token)
    {
        return token.Equals(UnderscoreToken) || token.Equals(DoubleUnderscoreToken);
    }

    private bool ShouldSkipEmphasis(IToken? previousToken, IToken nextToken)
    {
        return nextToken.Type is TokenType.Space || previousToken?.Type is TokenType.Space;
    }

    private void HandleClosingEmphasis(IToken token, Stack<IToken> tagStack, Stack<IToken> handledStack)
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