using ReactCoreTestApp.Server.Data;
using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly DocumentContext _context;
        public DocumentService(ILogger<DocumentService> logger, DocumentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Document AddDocument(Document document)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDocument(string id)
        {
            throw new NotImplementedException();
        }

        public Document GetDocument(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Document> GetDocuments()
        {
            throw new NotImplementedException();
        }

        public Document UpdateDocumet(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
