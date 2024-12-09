using Markdown.Markdown.Handlers;
using Markdown.Markdown.Handlers.Emphasis;

namespace Markdown.Markdown;

public static class TokenHandlerFactory
{
    private static readonly ITokenHandler[] Handlers =
    [
        new TagTypeHandler(),
        new EscapeSequenceHandler(),
        new EmphasisHandler(),
        new HeaderHandler(),
        new NonPairTagHandler()
    ];

    public static IEnumerable<ITokenHandler> CreateHandlers() => Handlers;
}