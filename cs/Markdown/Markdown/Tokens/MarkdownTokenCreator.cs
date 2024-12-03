using Markdown.Html.Tags;

namespace Markdown.Markdown.Tokens;

internal static class MarkdownTokenCreator
{
    public static IToken CreateCloseTag(IToken token, TagType tagType) =>
        new MarkdownToken(token.Content, token.Type, tagType, IsCloseTag: true);

    public static IToken CreateTextToken(string content) =>
        new MarkdownToken(content, TokenType.Text);

    public static bool TryCreateSymbolToken(string content, out IToken? token)
    {
        if (int.TryParse(content, out _))
        {
            token = new MarkdownToken(content, TokenType.Digit);
            return true;
        }

        token = content switch
        {
            " " => new MarkdownToken(content, TokenType.Space),
            "\\" => new MarkdownToken(content, TokenType.Escape),
            _ => null
        };

        return token != null;
    }

    public static IToken NewLine =>
        new MarkdownToken("\n", TokenType.NewLine);

    public static IToken CreateTag(string content, TagType tagType) =>
        new MarkdownToken(content, TokenType.Tag, tagType);
}