using ChromaDBSharp.Client;
using ChromaDBSharp.Embeddings;
using Microsoft.Extensions.Options;
using ReactCoreTestApp.Server.Data;
using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly DocumentContext _context;
        private readonly IChromaDBClient _chromaClient;
        private readonly ICollectionClient _collectionClient;
        private readonly ITextSplitter _textSplitter;
        public DocumentService(ILogger<DocumentService> logger, 
            DocumentContext context,
            IChromaDBClient chromaDBClient,
            IEmbeddable embedder,
            IOptions<AppSettings> options, 
            ITextSplitter textSplitter)
        {
            _logger = logger;
            _context = context;
            _chromaClient = chromaDBClient;
            _collectionClient = chromaDBClient.GetCollection(options.Value.ChromaDocumentCollection, embedder);
            _textSplitter = textSplitter;
        }

        public Document AddDocument(Document document)
        {
            // Split the document into chunks.
            if (string.IsNullOrWhiteSpace(document.Text))
            {
                throw new Exception("Document has no text");
            }
            document.Id = Guid.NewGuid().ToString();

            IEnumerable<string> chunks = _textSplitter.Split(document.Text);

            // Add chunks to Chroma.
            _collectionClient.Add(ids: chunks.Select(_ => Guid.NewGuid().ToString()),
                metadatas: chunks.Select(_ => new Dictionary<string, object>
                {
                    { "id", document.Id},
                    { "title", document.Title},
                    { "description", document.Description},
                    { "type", document.Type.ToString()}
                }), 
                documents: chunks);

            // Add document to DB.
            _context.Add(document);
            _context.SaveChanges();

            return document;
        }

        public bool DeleteDocument(string id)
        {
            // Remove from Chroma.
            _collectionClient.Delete(where: new
            Dictionary<string, object> {
                { "id", id}
            });

            var entry = _context.Documents.Find(id);
            if (entry != null)
            {
                _context.Documents.Remove(entry);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public Document? GetDocument(string id)
        {
            return _context.Documents.Find(id);
        }

        public IEnumerable<Document> GetDocuments()
        {
            return _context.Documents;
        }

        public IEnumerable<QueryResponseDTO> Query(string query)
        {
            var chromaDocuments = _collectionClient.Query(queryTexts: [query], numberOfResults: 3, include: ["metadatas", "documents", "distances"]);

            var results = chromaDocuments.Documents.First().Zip(chromaDocuments.Metadatas.First()).Zip(chromaDocuments.Distances.First()).Select(s =>
            {
                return new ChromaQueryResult
                {
                    Document = s.First.First,
                    Metadata = s.First.Second,
                    Distance = s.Second
                };
            })
            .Where(r => r.Distance < 1f);

            var docIds = results.Where(r =>
            {
                if (r.Metadata != null && r.Metadata.ContainsKey("id"))
                {
                    return true;
                }

                return false;
            }).Select(r => r.Metadata["id"]);

            List<Document> docs = [];
            foreach (var id in docIds)
            {
                var entity = _context.Documents.Find(id);
                if(entity != null)
                {
                    docs.Add(entity);
                }
            }

            return results.Zip(docs).Select(zipped =>
            {
                return new QueryResponseDTO
                {
                    Text = zipped.First.Document,
                    Document = zipped.Second
                };
            });
            
        }

        public Document UpdateDocumet(Document document)
        {
            // Find document.
            Document? dbDocument = _context.Documents.Find(document.Id) 
                ?? throw new Exception($"Document not found {document.Id}");

            // Split the document into chunks.
            if (string.IsNullOrWhiteSpace(document.Text))
            {
                throw new Exception("Document has no text");
            }

            dbDocument.Text = document.Text;
            dbDocument.Author = document.Author;
            dbDocument.Content = document.Content;
            dbDocument.Description = document.Description;
            dbDocument.Title = document.Title;

            _context.Update(dbDocument);
            _context.SaveChanges();

            IEnumerable<string> chunks = _textSplitter.Split(dbDocument.Text);


            // Add chunks to Chroma.
            _collectionClient.Add(ids: chunks.Select(_ => Guid.NewGuid().ToString()),
                metadatas: [new Dictionary<string, object>
                {
                    { "id", dbDocument.Id},
                    { "title", dbDocument.Title},
                    { "description", dbDocument.Description},
                    { "type", dbDocument.Type.ToString()}
                }],
                documents: chunks);

            return dbDocument;
        }
    }
}
