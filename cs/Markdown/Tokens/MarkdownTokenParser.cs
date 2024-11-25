namespace Markdown.Tokens;

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

    private IEnumerable<MarkdownToken> ParseLines(string[] lines)
    {
        foreach (var line in lines)
        {
            foreach (var token in ParseLine(line))
            {
                yield return token;
            }
            yield return MarkdownToken.GetToken("\n");
        }
    }

    private static IEnumerable<MarkdownToken> ParseLine(string line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            //TODO: Здесь будет реализация разделения строки на токены
            var currStr = line.Substring(i, 1);

            var isTagToken = MarkdownToken.TryGetTagToken(currStr, out var tagToken);
            if (isTagToken)
                yield return tagToken;
            else
                yield return MarkdownToken.GetToken(currStr);
        }
    }
}