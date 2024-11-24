using Markdown.Html;

namespace Markdown.Tokens;

public class MarkdownTokenParser
{
    private readonly Dictionary<string, IHtmlTag> _markdownTags = new()
    {
        {"_", new ItalicTag()},
        {"__", new BoldTag()},
        {"#", new HeaderTag()}
    };

    public IEnumerable<Token> Parse(string text)
    {
        var lines = SplitIntoLines(text);
        return ParseLines(lines);
    }

    private static string[] SplitIntoLines(string text)
    {
        return text.Split(['\n', '\r']);
    }

    private IEnumerable<Token> ParseLines(string[] lines)
    {
        foreach (var line in lines)
        {
            foreach (var token in ParseLine(line))
            {
                yield return token;
            }
            yield return CreateNewLineToken();
        }
    }

    private IEnumerable<Token> ParseLine(string line)
    {
        var words = SplitIntoWords(line);
        foreach (var word in words)
        {
            yield return ParseWord(word);
            yield return CreateSpaceToken();
        }
    }

    private static string[] SplitIntoWords(string line)
    {
        return line.Split();
    }

    private Token ParseWord(string word)
    {
        return TryParseMarkdownTag(word, out var token) 
            ? token 
            : CreateTextToken(word);
    }

    private bool TryParseMarkdownTag(string value, out Token token)
    {
        token = CreateTextToken(value);

        var matchingTag = FindMatchingTag(value);
        if (matchingTag == null)
            return false;

        token = matchingTag.ToHtml(value);
        return true;
    }

    private IHtmlTag? FindMatchingTag(string value)
    {
        return _markdownTags
            .Where(tag => value.StartsWith(tag.Key))
            .Select(tag => tag.Value)
            .FirstOrDefault();
    }

    private static Token CreateTextToken(string value)
    {
        return new Token(value, TokenType.Text);
    }

    private static Token CreateSpaceToken()
    {
        return new Token(" ", TokenType.Space);
    }

    private static Token CreateNewLineToken()
    {
        return new Token("\n", TokenType.NewLine);
    }
}