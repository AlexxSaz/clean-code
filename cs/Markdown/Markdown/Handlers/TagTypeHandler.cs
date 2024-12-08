using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class TagTypeHandler : ITokenHandler
{
    public IList<IToken> Handle(IList<IToken> tokens)
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            var tagType = tokens[i].Content switch
            {
                "_" => TagType.Italic,
                "__" => TagType.Strong,
                "# " => TagType.Header,
                _ => TagType.None
            };
            if (tagType is not TagType.None)
                tokens[i] = MarkdownTokenCreator.CreateOpenTag(tokens[i], tagType);
        }

        return tokens;
    }
}