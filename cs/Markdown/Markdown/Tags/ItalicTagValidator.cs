namespace Markdown.Markdown.Tags;

public class ItalicTagValidator() : UnderscoreTagValidatorBase("_")
{
    public override bool IsValidTag(string content) =>
        content == UnderscoreContent;
}