using Markdown.Html.Tags;

namespace Markdown.Markdown.Tokens;

public record MarkdownToken(string Content, TokenType Type, TagType TagType = TagType.None, bool IsCloseTag = false) : IToken;