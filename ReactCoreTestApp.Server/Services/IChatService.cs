using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Services
{
    public interface IChatService
    {
        ChatResponse Chat(ChatRequest chatRequest);
    }
}
