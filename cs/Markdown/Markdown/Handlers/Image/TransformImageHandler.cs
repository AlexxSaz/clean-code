using System.Text;
using Markdown.Markdown.Attributes;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Image;

public class TransformImageHandler : ImageHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledAttributes = new HashSet<IAttribute>();
        var startIndex = -1;
        var endIndex = -1;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (!IsImageTag(token)) continue;
            startIndex = i;

            var altAttribute = AttributeCreator.CreateImageAttribute(AttributeType.Alt,
                GetAttributeContent(tokens, token, ref i));

            var srcAttribute = AttributeCreator.CreateImageAttribute(AttributeType.Src,
                GetAttributeContent(tokens, token, ref i));
            handledAttributes.Add(altAttribute);
            handledAttributes.Add(srcAttribute);
            endIndex = i;
        }

        return handledAttributes.Count != 2
            ? tokens
            : ReplaceImageTagsByOne(tokens,
                MarkdownTokenCreator.CreateTagWithAttributes(tokens[startIndex], handledAttributes), startIndex,
                endIndex);
    }

    private static string GetAttributeContent(IReadOnlyList<IToken> tokens, IToken token, ref int i)
    {
        var attributeContent = new StringBuilder();
        var lastToken = tokens[i];
        while (i < tokens.Count && tokens[++i].TagPair != lastToken)
            attributeContent.Append(tokens[i].Content);
        return attributeContent.ToString();
    }

    private static List<IToken> ReplaceImageTagsByOne(IReadOnlyList<IToken> tokens, IToken newImageTag, int startIndex,
        int endIndex)
    {
        var handledTokens = new List<IToken>();
        for (var i = 0; i < startIndex; i++)
            handledTokens.Add(tokens[i]);
        handledTokens.Add(newImageTag);
        for (var i = endIndex + 1; i < tokens.Count; i++)
            handledTokens.Add(tokens[i]);
        return handledTokens;
    }
}