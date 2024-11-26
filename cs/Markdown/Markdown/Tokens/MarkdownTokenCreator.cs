namespace Markdown.Markdown.Tokens;

public class MarkdownTokenCreator
{
    public static MarkdownToken CreateSymbolToken(string content)
    {
        return content switch
        {
            " " => new MarkdownToken(content, TokenType.Space),
            "_" => new MarkdownToken(content, TokenType.TagPart),
            "#" => new MarkdownToken(content, TokenType.Header),
            "\n" => new MarkdownToken(content, TokenType.NewLine),
            "\r" => new MarkdownToken(content, TokenType.NewLine),
            "\\" => new MarkdownToken(content, TokenType.Escape),
            _ => new MarkdownToken(content, TokenType.Letter)
        };
    }
}