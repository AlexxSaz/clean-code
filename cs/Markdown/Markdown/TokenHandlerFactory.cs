using Markdown.Markdown.Handlers;
using Markdown.Markdown.Handlers.Emphasis;

namespace Markdown.Markdown;

public static class TokenHandlerFactory
{
    private static readonly ITokenHandler[] _handlers =
    [
        new TagTypeHandler(),
        new EscapeSequenceHandler(),
        new EmphasisHandler(),
        new HeaderHandler()
    ];

    public static IEnumerable<ITokenHandler> CreateHandlers() => _handlers;
}