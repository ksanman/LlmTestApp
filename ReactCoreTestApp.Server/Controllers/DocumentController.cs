using Microsoft.AspNetCore.Mvc;
using ReactCoreTestApp.Server.Models;
using ReactCoreTestApp.Server.Services;

namespace ReactCoreTestApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentService _documentService;
        public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService) 
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet]
        public IActionResult GetDocuments()
        {
            try
            {
                IEnumerable<Document> documents = _documentService.GetDocuments();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting documents");
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDocument(string id)
        {
            try
            {
                Document document = _documentService.GetDocument(id);
                return Ok(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting documents");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult AddDocument(Document document)
        {
            try
            {
                Document newDocument = _documentService.AddDocument(document);
                return Ok(newDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting documents");
                return StatusCode(500);
            }
        }

        [HttpPut]
        public IActionResult UpdateDocument(string id, [FromBody] Document document)
        {
            try
            {
                if (id != document.Id)
                {
                    return Unauthorized();
                }

                Document updatedDocument = _documentService.UpdateDocumet(document);
                return Ok(updatedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting documents");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public IActionResult DeleteDocument(string id)
        {
            try
            {
                _documentService.DeleteDocument(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting documents");
                return StatusCode(500);
            }
        }
    }
}
