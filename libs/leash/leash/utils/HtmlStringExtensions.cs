using HtmlAgilityPack;
using ReverseMarkdown;

namespace leash.utils;

public static class HtmlStringExtensions
{
    private readonly static Converter HtmlToMarkdownConverter = new();

    public static string FromHtmlToMarkdown(this string html) =>
        HtmlToMarkdownConverter.Convert(html);

    public static HtmlDocument? ToHtmlDocument(this string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc.ParseErrors != null && !doc.ParseErrors.Any() ? doc : null;
    }
}