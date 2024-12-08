using System.Collections;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public interface ITokenHandler
{
    IList<IToken> Handle(IList<IToken> tokens);
}