using Markdown.Html.Tags;

namespace Markdown.Markdown.Tokens;

public interface IToken
{
    TokenType Type { get; }
    string Content { get; }
    bool IsCloseTag { get; }
    TagType TagType { get; init; }
}