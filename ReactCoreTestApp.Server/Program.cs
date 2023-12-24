using AllMiniLmL6V2Sharp;
using ChromaDBSharp;
using ChromaDBSharp.Embeddings;
using Docnet.Core;
using LLama;
using LLama.Common;
using Microsoft.EntityFrameworkCore;
using ReactCoreTestApp.Server;
using ReactCoreTestApp.Server.Data;
using ReactCoreTestApp.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DocumentContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("documentsdb"));
});
AppSettings settings = builder.Configuration.GetSection(AppSettings.AppSettingsName).Get<AppSettings>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.AppSettingsName));
builder.Services.AddTransient<IDocumentParserFactory, DocumentParserFactory>();
builder.Services.AddTransient<ITextSplitter, RecursiveTextSplitter>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IEmbedder, AllMiniLmL6V2Embedder>(providers => new AllMiniLmL6V2Embedder(modelPath: settings.AllMiniV2Model, tokenizer: new AllMiniLmL6V2Sharp.Tokenizer.BertTokenizer(settings.AllMiniV2Vocab)));
builder.Services.AddScoped<IEmbeddable, AllMiniEmbedding>();
builder.Services.AddSingleton<IDocLib, DocLib>(providers => DocLib.Instance);
builder.Services.RegisterChromaDBSharp(settings.ChromaDbUrl);

// Load a model
var parameters = new ModelParams(settings.ChatModelPath)
{
    ContextSize = 2048,
    Seed = 1337,
    GpuLayerCount = 5
};
builder.Services.AddSingleton(services => LLamaWeights.LoadFromFile(parameters));
builder.Services.AddSingleton(services => services.GetRequiredService<LLamaWeights>().CreateContext(parameters));
builder.Services.AddScoped(services =>
{
    LLamaContext context = services.GetRequiredService<LLamaContext>();
    var ex = new InteractiveExecutor(context);
    return new ChatSession(ex);
});

builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

DocumentContext context = app.Services.GetRequiredService<DocumentContext>();
context.Database.EnsureCreated();

app.Run();
