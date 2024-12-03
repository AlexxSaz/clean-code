using System.Collections;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public interface ITokenHandler
{
    List<IToken> Handle(IList<IToken> tokens);
}