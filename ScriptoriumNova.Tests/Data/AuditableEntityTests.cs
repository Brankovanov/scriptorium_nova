using Scriptorium_Nova.Web.Data;
using Scriptorium_Nova.Web.Data.Models;
using ScriptoriumNova.Tests.Infrastructure;

namespace ScriptoriumNova.Tests.Data;

public class AuditableEntityTests : IDisposable
{
    private readonly TestDbFactory factory = new();

    public void Dispose() => factory.Dispose();

    private static Entry CreateEntry(string name = "Test entry") => new()
    {
        Name = name,
        Summary = "A test summary",
        EntryTypeId = 1
    };

    private int SeedEntry()
    {
        using ApplicationDbContext context = factory.CreateContext();
        Entry entry = CreateEntry();
        context.Entries.Add(entry);
        context.SaveChanges();
        return entry.Id;
    }

    [Fact]
    public async Task SaveChanges_AddedEntry_HasCreatedOnAndNoModifiedOn()
    {
        DateTime before = DateTime.UtcNow;
        int id;

        await using (ApplicationDbContext context = factory.CreateContext())
        {
            Entry entry = CreateEntry();
            context.Entries.Add(entry);
            await context.SaveChangesAsync();
            id = entry.Id;
        }

        await using ApplicationDbContext verify = factory.CreateContext();
        Entry saved = await verify.Entries.FindAsync(id) ?? throw new InvalidOperationException();

        Assert.True(saved.CreatedOn >= before);
        Assert.Null(saved.ModifiedOn);
    }

    [Fact]
    public async Task SaveChangesAsync_EditedEntry_SetsModifiedOn()
    {
        int id = SeedEntry();
        DateTime before = DateTime.UtcNow;

        await using (ApplicationDbContext context = factory.CreateContext())
        {
            Entry entry = await context.Entries.FindAsync(id) ?? throw new InvalidOperationException();
            entry.Summary = "A changed summary";
            await context.SaveChangesAsync();
        }

        await using ApplicationDbContext verify = factory.CreateContext();
        Entry saved = await verify.Entries.FindAsync(id) ?? throw new InvalidOperationException();

        Assert.NotNull(saved.ModifiedOn);
        Assert.True(saved.ModifiedOn >= before);
    }

    [Fact]
    public void SaveChanges_EditedEntry_SetsModifiedOnForSyncOverload()
    {
        int id = SeedEntry();

        using (ApplicationDbContext context = factory.CreateContext())
        {
            Entry entry = context.Entries.Find(id)!;
            entry.Name = "Renamed entry";
            context.SaveChanges();
        }

        using ApplicationDbContext verify = factory.CreateContext();
        Assert.NotNull(verify.Entries.Find(id)!.ModifiedOn);
    }

    [Fact]
    public async Task SaveChangesAsync_NothingChanged_LeavesModifiedOnNull()
    {
        int id = SeedEntry();

        await using (ApplicationDbContext context = factory.CreateContext())
        {
            _ = await context.Entries.FindAsync(id);
            await context.SaveChangesAsync();
        }

        await using ApplicationDbContext verify = factory.CreateContext();
        Assert.Null((await verify.Entries.FindAsync(id))!.ModifiedOn);
    }

    [Fact]
    public async Task SaveChangesAsync_DetachedUpdate_DoesNotOverwriteCreatedOn()
    {
        int id = SeedEntry();

        DateTime originalCreatedOn;
        await using (ApplicationDbContext read = factory.CreateContext())
        {
            originalCreatedOn = (await read.Entries.FindAsync(id))!.CreatedOn;
        }

        await using (ApplicationDbContext context = factory.CreateContext())
        {
            Entry detached = CreateEntry("Detached edit");
            detached.Id = id;
            detached.CreatedOn = DateTime.UtcNow.AddYears(-5);

            context.Update(detached);
            await context.SaveChangesAsync();
        }

        await using ApplicationDbContext verify = factory.CreateContext();
        Entry saved = (await verify.Entries.FindAsync(id))!;

        Assert.Equal(originalCreatedOn, saved.CreatedOn);
        Assert.Equal("Detached edit", saved.Name);
        Assert.NotNull(saved.ModifiedOn);
    }
}