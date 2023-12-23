using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetDocuments();
        Document? GetDocument(string id);
        Document AddDocument(Document document);
        Document UpdateDocumet(Document document);
        bool DeleteDocument(string id);
        IEnumerable<QueryResponseDTO> Query(string query);
    }
}
