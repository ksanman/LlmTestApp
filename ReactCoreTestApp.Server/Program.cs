using AllMiniLmL6V2Sharp;
using ChromaDBSharp;
using ChromaDBSharp.Embeddings;
using Docnet.Core;
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

app.Run();
