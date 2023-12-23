using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services.DocumentParser
{
    public sealed class TextDocumentParser : IDocumentParser
    {
        private readonly Document _document;
        public TextDocumentParser(Document document) 
        {
            _document = document;
        }

        public string ParseText()
        {
            return _document.Content;
        }
    }
}
