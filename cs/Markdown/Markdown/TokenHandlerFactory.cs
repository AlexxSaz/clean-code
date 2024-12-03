using Markdown.Markdown.Handlers;

namespace Markdown.Markdown;

public static class TokenHandlerFactory
{
    private static readonly ITokenHandler[] _handlers =
    [
        new EscapeSequenceHandler(),
        new EmphasisHandler(),
        new HeaderHandler()
    ];

    public static IEnumerable<ITokenHandler> CreateHandlers() => _handlers;
}