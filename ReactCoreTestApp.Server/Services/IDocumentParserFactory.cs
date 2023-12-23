using ReactCoreTestApp.Server.Models;
using ReactCoreTestApp.Server.Services.DocumentParser;

namespace ReactCoreTestApp.Server.Services
{
    public interface IDocumentParserFactory
    {
        IDocumentParser Create(Document document);
    }
}
