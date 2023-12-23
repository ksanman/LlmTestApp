using HtmlAgilityPack;
using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services.DocumentParser
{
    public class UrlParser : IDocumentParser
    {
        private readonly Document _document;
        public UrlParser(Document document)
        {
            _document = document;
        }

        public string ParseText()
        {
            HtmlDocument html = new();
            html.Load(_document.Content);
            string text = html.DocumentNode.InnerText;
            return text;
        }
    }
}
