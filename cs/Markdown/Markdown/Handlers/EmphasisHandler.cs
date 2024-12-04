using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class EmphasisHandler : ITokenHandler
{
    private static readonly IToken UnderscoreToken = new MarkdownToken("_", TokenType.Tag);
    private static readonly IToken DoubleUnderscoreToken = new MarkdownToken("__", TokenType.Tag);

    public List<IToken> Handle(IList<IToken> tokens)
    {
        var stack = new Stack<IToken>();
        var handledTokens = new List<IToken>();
        var bufferTokens = new List<IToken>();
        foreach (var token in tokens)
        {
            if (stack.Count > 0 && stack.Peek().Equals(token))
            {
                var tagType = token.Content == "_" ? TagType.Italic : TagType.Strong;
                var openTag = MarkdownTokenCreator.CreateOpenTag(stack.Pop(), tagType);
                var closeTag = MarkdownTokenCreator.CreateCloseTag(token, tagType);


                if (stack.Count > 0)
                {
                    var currBuff = new List<IToken>(bufferTokens);
                    bufferTokens =
                    [
                        openTag
                    ];
                    bufferTokens.AddRange(currBuff);
                    bufferTokens.Add(closeTag);
                }
                else
                {
                    handledTokens.Add(openTag);
                    handledTokens.AddRange(bufferTokens);
                    handledTokens.Add(closeTag);
                }

                
                continue;
            }

            if (token.Equals(UnderscoreToken))
            {
                stack.Push(UnderscoreToken);
                handledTokens.AddRange(bufferTokens);
                bufferTokens = [];
                continue;
            }

            if (token.Equals(DoubleUnderscoreToken))
            {
                stack.Push(DoubleUnderscoreToken);
                handledTokens.AddRange(bufferTokens);
                bufferTokens = [];
                continue;
            }

            bufferTokens.Add(token);
        }

        if (bufferTokens.Count > 0)
            handledTokens.AddRange(bufferTokens);
        return handledTokens;
    }
}