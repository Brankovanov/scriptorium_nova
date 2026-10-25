using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data.Models;

namespace Scriptorium_Nova.Web.Data.Seeding;

public static class DatabaseSeeder
{
    // Ids match the HasData seed in EntryTypeConfiguration.
    private const int FactionTypeId = 1;
    private const int LocationTypeId = 2;
    private const int CharacterTypeId = 3;
    private const int ArtifactTypeId = 4;
    private const int EventTypeId = 5;

    public static async Task SeedAsync(IServiceProvider services)
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        if (await context.Entries.AnyAsync())
        {
            return;
        }

        List<Entry> entries = new List<Entry>
        {
            new Entry
            {
                Name = "The Ashen Court",
                Summary = "A secretive council that rules the northern marches from the shadows.",
                Details = "Founded after the collapse of the old kingdom, the Court trades in debts and favours rather than armies.",
                EntryTypeId = FactionTypeId,
                Notes = new List<Note>
                {
                    new Note { Title = "Open question", Text = "Who funds the Court's couriers? Decide before the second arc." },
                    new Note { Title = "Rivalry", Text = "Natural enemy of the Salt Wardens because of the toll disputes." }
                }
            },
            new Entry
            {
                Name = "Salt Wardens",
                Summary = "Coastal guild that controls every harbour toll on the western shore.",
                EntryTypeId = FactionTypeId
            },
            new Entry
            {
                Name = "Hollow Spire",
                Summary = "A broken tower on the cliffs where the old maps say the sky was anchored.",
                Details = "Visible from three days away. Locals refuse to climb past the third landing.",
                EntryTypeId = LocationTypeId,
                Notes = new List<Note>
                {
                    new Note { Title = "Atmosphere", Text = "Always wind, never birdsong. Use the silence as a warning sign." }
                }
            },
            new Entry
            {
                Name = "Port Vael",
                Summary = "Crowded harbour city where every faction keeps a quiet embassy.",
                EntryTypeId = LocationTypeId
            },
            new Entry
            {
                Name = "Maren Duskwell",
                Summary = "A disgraced cartographer who knows where the Spire's missing floors went.",
                Details = "Maren sells copies of her maps to anyone, and never tells the same lie twice.",
                EntryTypeId = CharacterTypeId,
                Notes = new List<Note>
                {
                    new Note { Title = "Voice", Text = "Dry, precise, always correcting other people's directions." },
                    new Note { Title = "Secret", Text = "She drew the first map of the Spire from a dream, not a survey." },
                    new Note { Title = "Role", Text = "Guide character for the first act; leaves the party at Port Vael." }
                }
            },
            new Entry
            {
                Name = "Captain Orrin Vale",
                Summary = "Veteran Salt Warden captain with a quiet grudge against the Court.",
                EntryTypeId = CharacterTypeId
            },
            new Entry
            {
                Name = "The Lantern of Thessaly",
                Summary = "A brass lantern that shows the last place its holder felt safe.",
                Details = "Its light is cold and does not cast shadows. Three known owners, all of whom disappeared.",
                EntryTypeId = ArtifactTypeId
            },
            new Entry
            {
                Name = "The Night of Open Doors",
                Summary = "The single night when every gate in Port Vael was found unlocked.",
                Details = "No theft was reported. The cause was never found.",
                EntryTypeId = EventTypeId,
                Notes = new List<Note>
                {
                    new Note { Title = "Theory", Text = "Possibly the Court testing how the city reacts to an absent guard." }
                }
            }
        };

        await context.Entries.AddRangeAsync(entries);
        await context.SaveChangesAsync();
    }
}