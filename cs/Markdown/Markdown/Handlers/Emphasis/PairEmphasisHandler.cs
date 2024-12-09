using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class PairEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var lastItalicIndex = -1;
        var lastStrongIndex = -1;
        var handledTokens = new List<IToken>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (lastItalicIndex > -1 && token.TagType is TagType.Italic)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateCloseTag(token, token.TagType, tokens[lastItalicIndex]));
                lastItalicIndex = -1;
                continue;
            }

            if (lastStrongIndex > -1 && token.TagType is TagType.Strong)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateCloseTag(token, token.TagType, tokens[lastStrongIndex]));
                lastStrongIndex = -1;
                continue;
            }

            if (IsEmphasisTag(token))
            {
                switch (token.TagType)
                {
                    case TagType.Italic:
                        lastItalicIndex = i;
                        break;
                    case TagType.Strong:
                        lastStrongIndex = i;
                        break;
                }
            }
            handledTokens.Add(token);
        }

        return handledTokens;
    }
}