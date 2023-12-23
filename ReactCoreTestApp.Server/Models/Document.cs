namespace ReactCoreTestApp.Server.Models
{
    public enum DocumentType
    {
        Text = 0,
        PDF = 1,
        HTML = 2,
        URL = 3
    }

    public class Document
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DocumentType Type { get; set; } = DocumentType.PDF;
        public Document() { }
    }
}
