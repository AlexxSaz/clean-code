using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class NonPairTagHandler : ITokenHandler
{
    public IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledTokens = new List<IToken>();
        var tagStack = new Stack<IToken>();
        var indexTagQueue = new Queue<int>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                tagStack.Pop();
                indexTagQueue.Dequeue();
                continue;
            }
            
            if (token.Type is TokenType.Tag)
            {
                indexTagQueue.Enqueue(i);
                tagStack.Push(token);
            }
        }

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (indexTagQueue.Count > 0 && indexTagQueue.Peek() == i)
            {
                indexTagQueue.Dequeue();
                handledTokens.Add(MarkdownTokenCreator.CreateTextToken(token.Content));
                continue;
            }
            
            handledTokens.Add(token);
        }

        return handledTokens;
    }
}