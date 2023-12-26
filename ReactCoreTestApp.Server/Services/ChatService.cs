using LLama;
using LLama.Abstractions;
using LLama.Common;
using Microsoft.Extensions.Options;
using ReactCoreTestApp.Server.Models;
using System.Diagnostics;
using System.Text;

namespace ReactCoreTestApp.Server.Services
{
    public class ChatService : IChatService
    {
        //private readonly ChatSession _session;
        private readonly IDocumentService _documentService;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger<ChatService> _logger;
        private const string B_INST = "[INST]";
        private const string E_INST = "[/INST]";
        public ChatService(IDocumentService documentService, IOptions<AppSettings> settings, ILogger<ChatService> logger) 
        {
            //_session = chatSession;
            _documentService = documentService;
            _settings = settings;
            _logger = logger;
        }
        const string PROMPT = "<s>[INST] <<SYS>>\\nUsing the context provided, answer the question.\\n<</SYS>>\\n\\n Context: {0} \\n\\n Question: {1} [/INST]\r\n";
        public ChatResponse Chat(ChatRequest chatRequest)
        {
            _logger.LogTrace("Starting chat: {chat}", chatRequest.UserText);
            _logger.LogTrace("Searching for documents.");
            var documents = _documentService.Query(chatRequest.UserText);
            _logger.LogTrace("Found {count} documens", documents.Count());

            _logger.LogTrace("Building Prompt");
            var sources = string.Join("\\r\\n", documents.Select(d => d.Text));
            var prompt = string.Format(PROMPT, sources, chatRequest.UserText);
            StringBuilder responseBuilder = new();
            _logger.LogTrace("Prompt: {prompt}", prompt);

            _logger.LogTrace("Starting model");
            ModelParams parameters = new (_settings.Value.ChatModelPath)
            {
                ContextSize = 1024,
                Seed = 1337,
                GpuLayerCount = 5
            };
            using LLamaWeights weights = LLamaWeights.LoadFromFile(parameters);
            using LLamaContext context = weights.CreateContext(parameters);
            InstructExecutor executor = new(context, instructionPrefix: B_INST, instructionSuffix: E_INST);
            Stopwatch watch = Stopwatch.StartNew();
            var chatTask = Task.Run(async () =>
            {
                _logger.LogTrace("Fetching response");
                 await foreach (var text in executor.InferAsync(prompt, new InferenceParams() { Temperature = 0f, MaxTokens = 512 }))
                {
                    responseBuilder.Append(text);
                }

            });
            chatTask.Wait();
            watch.Stop();
            string response = responseBuilder.ToString();
            _logger.LogTrace("Received response in {time}: {response}", watch.Elapsed.ToString("hh\\:mm\\:ss\\.fff"), response);
            return new ChatResponse
            {
                ResponseText = response,
                Sources = documents.Select(d => d.Document).ToList()
            };
        }
    }
}
