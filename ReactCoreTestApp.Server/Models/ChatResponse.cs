using Newtonsoft.Json;

namespace ReactCoreTestApp.Server.Models
{
    public class ChatResponse
    {
        [JsonProperty("responseText")]
        public string ResponseText { get; set; } = string.Empty;
        [JsonProperty("sources")]
        public List<Document> Sources { get; set; } = new();
    }
}
