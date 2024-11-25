namespace Markdown.Tokens;

public class MarkdownToken(string content, TokenType type, bool isClosedTag = false)
{
    public string Content { get; } = content;
    public TokenType Type { get; } = type;
    public bool IsClosedTag { get; } = isClosedTag;

    public static bool TryGetTagToken(string str, out MarkdownToken resultToken)
    {
        throw new NotImplementedException();
    }

    public static MarkdownToken GetToken(string str)
    {
        throw new NotImplementedException();
    }
}