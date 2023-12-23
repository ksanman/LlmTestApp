using Microsoft.Extensions.Options;

namespace ReactCoreTestApp.Server.Services
{
    public sealed class RecursiveTextSplitter : ITextSplitter
    {
        private readonly string[] _separators = [" "];
        public RecursiveTextSplitter(IOptions<AppSettings> settings)
        {
            if(settings.Value?.Separators != null && settings.Value.Separators.Length > 0)
            {
                _separators = settings.Value.Separators;
            } 
        }
        public IEnumerable<string> Split(string text)
        {
            List<string> segments = [];
            RecursiveSegmentationHelper(text, segments);

            return segments;
        }

        private void RecursiveSegmentationHelper(string text, List<string> segments)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            int splitIndex = FindSplitIndex(text);
            segments.Add(text.Substring(0, splitIndex).Trim());

            if (splitIndex < text.Length)
            {
                RecursiveSegmentationHelper(text.Substring(splitIndex).Trim(), segments);
            }
        }

        private int FindSplitIndex(string text)
        {
            int index = text.Length;

            foreach (var separator in _separators)
            {
                int separatorIndex = text.LastIndexOf(separator, StringComparison.Ordinal);

                if (separatorIndex != -1 && separatorIndex < index)
                {
                    index = separatorIndex;
                }
            }

            return index;
        }
    }
}
