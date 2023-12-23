using HtmlAgilityPack;
using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services.DocumentParser
{
    public class HtmlTextParser : IDocumentParser
    {
        private readonly Document _document;
        public HtmlTextParser(Document document)
        {
            _document = document;
        }
        public string ParseText()
        {
            HtmlDocument html = new();
            html.LoadHtml(_document.Content);
            string text = html.DocumentNode.InnerText;
            return text;
        }
    }
}
