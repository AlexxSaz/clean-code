namespace Markdown.Markdown.Tokens;

public class MarkdownToken(string content, TokenType type, bool isClosedTag = false)
{
    public string Content { get; } = content;
    public TokenType Type { get; } = type;
    public bool IsClosedTag { get; } = isClosedTag;
}