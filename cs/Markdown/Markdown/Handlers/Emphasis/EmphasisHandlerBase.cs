using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public abstract class EmphasisHandlerBase : ITokenHandler
{
    public abstract IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens);

    protected static bool IsEmphasisTag(IToken token) =>
        token.TagType is TagType.Italic or TagType.Strong;
}