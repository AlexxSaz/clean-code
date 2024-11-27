using Markdown.Markdown.Handlers;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Processing;

public class TokenProcessingContext(IList<MarkdownToken> tokens) : IProcessingContext
{
    public IList<MarkdownToken> Tokens { get; } = tokens;
    public IList<MarkdownToken> Result { get; } = new List<MarkdownToken>();
    public int CurrentIndex { get; set; } = 0;
    public EmphasisLevel EmphasisLevel { get; set; } = EmphasisLevel.None;
    public bool EscapeNext { get; set; } = false;
    public bool HasMoreTokens =>
        CurrentIndex < Tokens.Count;
    public MarkdownToken? CurrentToken =>
        HasMoreTokens ? Tokens[CurrentIndex] : null;
}