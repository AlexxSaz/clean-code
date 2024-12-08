using Markdown.Html.Tags;

namespace Markdown.Markdown.Tokens;

public record MarkdownToken(
    string Content,
    TokenType Type,
    TagType TagType = TagType.None,
    IToken? TagPair = null,
    bool IsCloseTag = false) : IToken;