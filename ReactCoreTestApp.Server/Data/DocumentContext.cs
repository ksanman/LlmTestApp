using Microsoft.EntityFrameworkCore;
using ReactCoreTestApp.Server.Models;

namespace ReactCoreTestApp.Server.Data
{
    public class DocumentContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public DocumentContext(DbContextOptions<DocumentContext> context)
            : base(context)
        {

        }
    }
}
