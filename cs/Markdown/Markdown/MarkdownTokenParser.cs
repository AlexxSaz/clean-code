using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public class MarkdownTokenParser
{
    public IEnumerable<MarkdownToken> Parse(string text)
    {
        var lines = SplitIntoLines(text);
        return ParseLines(lines);
    }

    private static string[] SplitIntoLines(string text)
    {
        return text.Split('\n', '\r');
    }

    private static IEnumerable<MarkdownToken> ParseLines(string[] lines)
    {
        foreach (var line in lines)
        {
            foreach (var token in ParseLine(line))
            {
                yield return token;
            }
            yield return MarkdownTokenCreator.CreateSymbolToken("\n");
        }
    }

    private static IEnumerable<MarkdownToken> ParseLine(string line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            var currentSymbol = line.Substring(i, 1);

            yield return MarkdownTokenCreator.CreateSymbolToken(currentSymbol);
        }
    }
}