using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public abstract class EmphasisHandlerBase : ITokenHandler
{
    private static readonly IToken UnderscoreToken = new MarkdownToken("_", TokenType.Tag);
    private static readonly IToken DoubleUnderscoreToken = new MarkdownToken("__", TokenType.Tag);
    public abstract List<IToken> Handle(IList<IToken> tokens);

    protected static bool IsEmphasisTag(IToken token) =>
        token.Equals(UnderscoreToken) || token.Equals(DoubleUnderscoreToken);
}