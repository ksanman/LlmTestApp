namespace ReactCoreTestApp.Server
{
    public class AppSettings
    {
        public static string AppSettingsName => "AppSettings";
        public string ChromaDbUrl { get; set; } = string.Empty;
        public string ChromaDocumentCollection { get; set; } = string.Empty;
        public string[]? Separators { get; set; }
        public string AllMiniV2Vocab { get; set; } = string.Empty;
        public string AllMiniV2Model { get; set; } = string.Empty;
    }
}
