namespace Markdown.Markdown.Tags;

public class StrongTagValidator() : UnderscoreTagValidatorBase("__")
{
    public override bool IsValidTag(string content) =>
        content == UnderscoreContent;
}