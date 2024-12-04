using System.Text;
using Markdown.Html.Tags;
using Markdown.Markdown.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public class TextTokenizer : ITokenizer
{
    public IEnumerable<IToken> Tokenize(string text)
    {
        var lines = SplitIntoLines(text);
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            foreach (var token in TokenizeLine(line))
            {
                yield return token;
            }

            if (i < lines.Length - 1)
                yield return MarkdownTokenCreator.NewLine;
        }
    }

    private static string[] SplitIntoLines(string text) =>
        text.Split(MarkdownConstants.Symbols.Newline, MarkdownConstants.Symbols.CarriageReturn);

    private static IEnumerable<IToken> TokenizeLine(string line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            var currentSymbol = line.Substring(i, 1);

            if (MarkdownTokenCreator.TryCreateSymbolToken(currentSymbol, out var symbolToken))
                yield return symbolToken;
            else
            {
                var type = MarkdownTagValidator.IsTagStart(currentSymbol)
                    ? TokenType.Tag
                    : TokenType.Text;
                yield return CreateToken(line, type, ref i);
            }
        }
    }

    private static IToken CreateToken(string line, TokenType tokenType, ref int i)
    {
        var contentBuilder = new StringBuilder();

        while (i < line.Length)
        {
            var currentSymbol = line.Substring(i, 1);

            if (currentSymbol != "#" && IsTokenEnded(contentBuilder.ToString(), currentSymbol, tokenType))
                break;

            contentBuilder.Append(line[i]);
            i++;
        }

        i--;
        var content = contentBuilder.ToString();

        if (MarkdownTokenCreator.TryCreateTagToken(content, out var tagToken))
            return tagToken;

        return MarkdownTokenCreator.CreateTextToken(content);
    }

    private static bool IsTokenEnded(string content, string symbol, TokenType tokenType) =>
        (tokenType == TokenType.Text && (MarkdownTagValidator.IsTagStart(symbol) ||
                                         MarkdownTokenCreator.TryCreateSymbolToken(symbol, out _))) ||
        (tokenType == TokenType.Tag &&
         !MarkdownTokenCreator.TryCreateTagToken(content + symbol, out _));
}