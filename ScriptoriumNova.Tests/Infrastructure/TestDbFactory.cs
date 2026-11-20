using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data;

namespace ScriptoriumNova.Tests.Infrastructure;

public sealed class TestDbFactory : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public TestDbFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open(); // must stay open, or the in-memory DB vanishes

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public ApplicationDbContext CreateContext() => new(_options);

    public void Dispose() => _connection.Dispose();
}