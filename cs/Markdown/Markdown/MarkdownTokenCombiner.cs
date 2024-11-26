using System.Text;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public class MarkdownTokenCombiner
{

    public IList<MarkdownToken> Combine(IList<MarkdownToken> symbolTokens)
    {
        var resultTokens = new List<MarkdownToken>();
        for (var i = 0; i < symbolTokens.Count; i++)
        {
            var currToken = symbolTokens[i];
            if (currToken.Type is TokenType.Header && (i == 0 || symbolTokens[i - 1].Type is TokenType.NewLine))
            {
                var builder = new StringBuilder();
                resultTokens.Add(currToken);
                i++;
                while (currToken.Type is not TokenType.NewLine)
                {
                    builder.Append(currToken.Content);
                    currToken = symbolTokens[i++];
                }
                resultTokens.Add(new MarkdownToken("#", TokenType.Header, true));
                resultTokens.Add(currToken);
                i++;
            }

            var count = 0;

            while (currToken.Type is TokenType.TagPart)
            {
                currToken = symbolTokens[i++];
                count++;
            }

            var token = count switch
            {
                2 => new MarkdownToken("__", TokenType.Tag),
                1 => new MarkdownToken("_", TokenType.Tag),
                > 2 => new MarkdownToken(new string('_', count), TokenType.Letter)
            };

            //TODO: Ищем закрывающий тэг
            //TODO: Если закрывающий тэг есть, то оставляем открывающий, создаем закрывающий, между ними - текст
            //TODO: Если закрывающего тэга нет, то всё, что прочли - текст



            while (currToken.Type is TokenType.Letter)
            {

            }
        }


        throw new NotImplementedException();
    }
}