using System.Text;
using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public class TextTokenizer : ITokenizer
{
    public IEnumerable<IToken> Tokenize(string text)
    {
        foreach (var line in SplitIntoLines(text))
        {
            foreach (var token in TokenizeLine(line))
            {
                yield return token;
            }

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
                var type = currentSymbol.StartsWith(MarkdownConstants.TagSymbols.SingleUnderscore)
                    ? TokenType.Tag
                    : TokenType.Text;
                yield return CreateTokenAndMoveIndex(line, type, ref i);
            }
        }
    }

    private static IToken? CreateTokenAndMoveIndex(string line, TokenType tokenType, ref int i)
    {
        var contentBuilder = new StringBuilder();

        while (i < line.Length && !IsTokenEnded(tokenType, line[i]))
        {
            contentBuilder.Append(line[i]);
            i++;
        }

        i--;

        return tokenType switch
        {
            TokenType.Tag => MarkdownTokenCreator.CreateTag(contentBuilder.ToString(), TagType.None),
            TokenType.Text => MarkdownTokenCreator.CreateTextToken(contentBuilder.ToString()),
            _ => null
        };
    }

    private static bool IsTokenEnded(TokenType type, char currChar) =>
        (type is TokenType.Text && currChar == '_') ||
        (type is TokenType.Tag && currChar != '_');
}