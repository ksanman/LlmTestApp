using ChromaDBSharp.Embeddings;
using Newtonsoft.Json;
using System.Text;

namespace ReactCoreTestApp.Server.Services
{
    internal class SentenceTransformersRequest
    {
        [JsonProperty("strings")]
        public IEnumerable<string> Strings { get; set; } = Enumerable.Empty<string>();
    }

    internal class SentenceTransformersResponse
    {
        [JsonProperty("embeddings")]
        public IEnumerable<IEnumerable<float>> Embeddings { get; set; } = Enumerable.Empty<IEnumerable<float>>();   
    }
    public class SentenceTransformersEmbedding : IEmbeddable
    {
        private readonly HttpClient _client;
        public SentenceTransformersEmbedding(HttpClient client)
        {
            _client = client;
        }
        public async Task<IEnumerable<IEnumerable<float>>> Generate(IEnumerable<string> texts)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "get_embeddings")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new SentenceTransformersRequest
                    {
                        Strings = texts
                    }), Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await _client.SendAsync(message);
                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    SentenceTransformersResponse stResponse = JsonConvert.DeserializeObject<SentenceTransformersResponse>(content)
                        ?? throw new Exception("Unable to parse Sentence transformers response");
                    return stResponse.Embeddings;   
                }
                else
                {
                    throw new Exception(content);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to generate embeddings", ex);
            }
        }
    }
}
