using LLama;
using LLama.Common;
using ReactCoreTestApp.Server.Models;
using System.Text;

namespace ReactCoreTestApp.Server.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatSession _session;
        private readonly IDocumentService _documentService;
        public ChatService(ChatSession chatSession, IDocumentService documentService) 
        {
            _session = chatSession;
            _documentService = documentService;
        }
        const string PROMPT = "<s>[INST] <<SYS>>\\nUsing the sources provided, answer the users question.\\n<</SYS>>\\n\\n Sources: {0} \\n\\n User: {1} [/INST]\r\n";
        public ChatResponse Chat(ChatRequest chatRequest)
        {
            var documents = _documentService.Query(chatRequest.UserText);

            var sources = string.Join("\\r\\n", documents.Select(d => d.Text));
            var prompt = string.Format(PROMPT, sources, chatRequest.UserText);
            StringBuilder responseBuilder = new();
            var chatTask = Task.Run(async () =>
            {
                await foreach (var text in _session.ChatAsync(prompt, new InferenceParams() { Temperature = 0f, AntiPrompts = new List<string> { "User:" } }))
                {
                    responseBuilder.Append(text);
                }

            });
            chatTask.Wait();

            return new ChatResponse
            {
                ResponseText = responseBuilder.ToString(),
                Sources = documents.Select(d => d.Document).ToList()
            };
        }
    }
}
