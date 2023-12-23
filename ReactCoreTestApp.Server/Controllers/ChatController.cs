using Microsoft.AspNetCore.Mvc;
using ReactCoreTestApp.Server.Models;
using ReactCoreTestApp.Server.Services;

namespace ReactCoreTestApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IChatService _chatService;
        public ChatController(ILogger<ChatController> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }

        [HttpPost]
        public IActionResult Chat(ChatRequest request)
        {
            try
            {
                ChatResponse res = _chatService.Chat(request);
                return Ok(res);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during chat");
                return StatusCode(500);
            }
        }
    }
}
