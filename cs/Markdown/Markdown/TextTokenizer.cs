using System.Text;
using Markdown.Markdown.Handlers;
using Markdown.Markdown.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public class TextTokenizer : ITokenizer
{
    private readonly IEnumerable<ITokenHandler> handlers =
        TokenHandlerFactory.CreateHandlers();

    public IList<IToken> Tokenize(string text)
    {
        var lines = SplitIntoLines(text);
        var tokens = new List<IToken>();
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var tokenLine = TokenizeLine(line).ToList();

            tokenLine = handlers.Aggregate(tokenLine, (current, handler) => handler.Handle(current).ToList());

            if (i < lines.Length - 1)
                tokenLine.Add(MarkdownTokenCreator.NewLine);
            tokens.AddRange(tokenLine);
        }

        return tokens;
    }

    private static string[] SplitIntoLines(string text) =>
        text.Split(MarkdownConstants.Newline);

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

            if (IsTokenEnded(contentBuilder.ToString(), currentSymbol, tokenType))
                break;

            contentBuilder.Append(line[i]);
            i++;
        }

        i--;
        var content = contentBuilder.ToString();

        return MarkdownTokenCreator.TryCreateTagToken(content, out var tagToken)
            ? tagToken
            : MarkdownTokenCreator.CreateTextToken(content);
    }

    private static bool IsTokenEnded(string content, string symbol, TokenType tokenType) =>
        (tokenType == TokenType.Text && (MarkdownTagValidator.IsTagStart(symbol) ||
                                         MarkdownTokenCreator.TryCreateSymbolToken(symbol, out _))) ||
        (tokenType == TokenType.Tag &&
         !MarkdownTokenCreator.TryCreateTagToken(content + symbol, out _) &&
         (!MarkdownTagValidator.IsTagStart(symbol) || MarkdownTagValidator.IsTagEnd(symbol)));
}