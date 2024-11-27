using Markdown.Markdown.Handlers;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Processing;

public interface IProcessingContext
{
    IList<MarkdownToken> Tokens { get; }
    IList<MarkdownToken> Result { get; }
    int CurrentIndex { get; set; }
    EmphasisLevel EmphasisLevel { get; set; }
    bool EscapeNext { get; set; }
    bool HasMoreTokens { get; }
    MarkdownToken? CurrentToken { get; }
}