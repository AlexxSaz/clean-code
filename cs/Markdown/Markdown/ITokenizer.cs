using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public interface ITokenizer
{
    IEnumerable<IToken> Tokenize(string text);
}