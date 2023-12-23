namespace ReactCoreTestApp.Server.Services
{
    public interface ITextSplitter
    {
        IEnumerable<string> Split(string text);
    }
}
