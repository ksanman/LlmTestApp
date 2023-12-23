namespace ReactCoreTestApp.Server.Models
{
    public class ChatRequest
    {
        public string UserText { get; set; } = string.Empty;
        public IEnumerable<string> History { get; set; } = Enumerable.Empty<string>();
    }
}
