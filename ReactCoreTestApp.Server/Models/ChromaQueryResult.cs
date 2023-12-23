namespace ReactCoreTestApp.Server.Models
{
    public class ChromaQueryResult
    {
        public string Document { get; set; } = string.Empty;
        public IDictionary<string, string>? Metadata { get; set; } = null;
        public float Distance { get; set; } = 0f;
    }
}
