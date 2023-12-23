using AllMiniLmL6V2Sharp;
using ChromaDBSharp.Embeddings;

namespace ReactCoreTestApp.Server.Services
{
    public class AllMiniEmbedding : IEmbeddable
    {
        private readonly IEmbedder _embedder;
        public AllMiniEmbedding(IEmbedder embedder)
        {
            _embedder = embedder;
        }
        public async Task<IEnumerable<IEnumerable<float>>> Generate(IEnumerable<string> texts)
        {
            IEnumerable<IEnumerable<float>> result = _embedder.GenerateEmbeddings(texts);
            return await Task.FromResult(result);
        }
    }
}
