using Docnet.Core;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using ReactCoreTestApp.Server.Models;
using System.Text;

namespace ReactCoreTestApp.Server.Services.DocumentParser
{

    public class PdfDocumentParser : IDocumentParser
    {
        private readonly IDocLib _docLib;
        private readonly Document _document;
        public PdfDocumentParser(IDocLib docLib, Document document) 
        {
            _docLib = docLib;
            _document = document;
        }

        public string ParseText()
        {
            byte[] pdfBytes = Convert.FromBase64String(_document.Content);
            using IDocReader docReader = _docLib.GetDocReader(pdfBytes, new PageDimensions());

            int pages = docReader.GetPageCount();
            StringBuilder sb = new();
            for(int i = 0; i < pages; i++)
            {
                using IPageReader pageReader = docReader.GetPageReader(i);
                string pageText = pageReader.GetText();
                sb.AppendLine(pageText);
                sb.AppendLine();
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
