using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class SkipEmphasisHandler : EmphasisHandlerBase
{
    public override List<IToken> Handle(IList<IToken> tokens)
    {
        if (tokens.Count < 1)
            return tokens.ToList();

        var tagStack = new Stack<IToken>();
        var handlerStack = new Stack<IToken>();

        IToken? previousToken = null;
        var token = tokens[0];
        for (var i = 1; i < tokens.Count; i++)
        {
            var nextToken = tokens[i];

            if (tagStack.Count > 0 && IsEmphasisTag(token))
            {
                if (tagStack.Peek().Equals(token) && previousToken?.Type is TokenType.Space)
                {
                    var bufferStack = new Stack<IToken>();
                    var startTag = tagStack.Pop();
                    while (handlerStack.Count > 0 && handlerStack.Peek() != startTag)
                    {
                        bufferStack.Push(handlerStack.Pop());
                    }

                    handlerStack.Push(MarkdownTokenCreator.CreateTextToken(handlerStack.Pop().Content));
                    while (bufferStack.Count > 0)
                    {
                        handlerStack.Push(bufferStack.Pop());
                    }

                    handlerStack.Push(MarkdownTokenCreator.CreateTextToken(token.Content));
                }
                else
                {
                    handlerStack.Push(token);
                }
                previousToken = token;
                token = nextToken;
                continue;
            }

            if (IsEmphasisTag(token) && nextToken.Type is not TokenType.Space)
                tagStack.Push(token);
            else if (IsEmphasisTag(token) && nextToken.Type is TokenType.Space)
            {
                handlerStack.Push(MarkdownTokenCreator.CreateTextToken(token.Content));
                previousToken = token;
                token = nextToken;
                continue;
            }

            handlerStack.Push(token);
            previousToken = token;
            token = nextToken;
        }

        handlerStack.Push(token);
        return handlerStack.Reverse().ToList();
    }
}