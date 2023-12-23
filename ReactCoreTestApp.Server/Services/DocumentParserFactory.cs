using Docnet.Core;
using ReactCoreTestApp.Server.Models;
using ReactCoreTestApp.Server.Services.DocumentParser;

namespace ReactCoreTestApp.Server.Services
{
    public sealed class DocumentParserFactory : IDocumentParserFactory
    {
        private readonly IDocLib _docLib;
        public DocumentParserFactory(IDocLib docLib)
        {
            _docLib = docLib;
        }

        public IDocumentParser Create(Document document)
        {
            return document.Type switch
            {
                DocumentType.Text => new TextDocumentParser(document),
                DocumentType.PDF => new PdfDocumentParser(_docLib, document),
                DocumentType.HTML => new HtmlTextParser(document),
                DocumentType.URL => new UrlParser(document),    
                _ => throw new Exception($"No parser found for document type {document.Type}"),
            };
        }
    }
}
