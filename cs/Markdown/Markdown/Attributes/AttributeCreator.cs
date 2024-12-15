namespace Markdown.Markdown.Attributes;

public class AttributeCreator
{
    public static IAttribute CreateImageAttribute(AttributeType attributeType, string content) =>
        new ImageAttribute(attributeType, content);
}