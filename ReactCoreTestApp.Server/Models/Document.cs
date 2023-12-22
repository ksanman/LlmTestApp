namespace ReactCoreTestApp.Server.Models
{
    public enum DocumentType
    {
        PDF = 0,
        HTML = 1,
        URL = 2
    }

    public class Document
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DocumentType Type { get; set; } = DocumentType.PDF;
        public Document() { }
    }
}
