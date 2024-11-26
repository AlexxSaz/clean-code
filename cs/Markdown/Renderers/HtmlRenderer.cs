﻿using System.Text;
using Markdown.Html;
using Markdown.Markdown;
using Markdown.Markdown.Tokens;

namespace Markdown.Renderers;

public class HtmlRenderer : IRenderer
{
    private readonly HtmlTagConverter _tagConverter = new();

    public string Render(IList<MarkdownToken> tokens)
    {
        var tokenBuilder = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.Type is TokenType.Tag)
                tokenBuilder.Append(_tagConverter.Convert(token.Content, token.IsClosedTag));
            else
                tokenBuilder.Append(token.Content);
        }

        return tokenBuilder.ToString();
    }
}