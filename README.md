# Scriptorium Nova

Scriptorium Nova is a web application for worldbuilders. It keeps the entries of a fictional universe (factions, locations, characters, artifacts and events) organized in one place, lets you classify each entry by type, and lets you attach notes to every entry.

It is built with ASP.NET Core MVC and Entity Framework Core as the ASP.NET Fundamentals project at SoftUni.

## Features

- Browse all entries as cards, search them by name and filter them by type
- Create, view, edit and delete entries (full CRUD)
- Add and remove notes on an entry's details page (up to 20 notes per entry)
- Browse entry types and see which entries belong to each one
- Home page with a short introduction, entry counts per type and the most recently added entries
- Friendly error page and a custom 404 page
- Responsive layout that works on phone screens
- Client-side and server-side validation with friendly error messages

## Technologies

- .NET 10 and ASP.NET Core MVC
- Entity Framework Core 10 (code first, migrations) with SQL Server
- Razor views with a shared layout, partial views and sections
- Bootstrap 5, plus a custom theme in `wwwroot/css/site.css`
- jQuery Validation and Unobtrusive Validation for client-side checks

## Architecture

The solution is one web project, organized by responsibility:

```
Scriptorium_Nova.Web/
  Common/          Validation constants and the custom validation attribute
  Controllers/     Home, Entries, EntryTypes, Notes
  Data/            DbContext, entity models, fluent configurations, seeding
  Migrations/      EF Core migrations
  Services/        Service interfaces and implementations
  ViewModels/      View models, grouped by feature
  Views/           Razor views and shared partials
```

- **Entities:** `EntryType` (one type has many entries), `Entry` (the main entity, belongs to one type and has many notes) and `Note` (belongs to one entry).
- **Services and dependency injection:** `IEntryService`, `IEntryTypeService` and `INoteService` are registered with `AddScoped`. Controllers depend only on these interfaces and never use the `DbContext` directly.
- **View models:** services return view models, so entities never reach the views.
- **Delete behavior:** deleting an entry also deletes its notes (cascade). Entry types cannot be deleted while entries use them (restrict), and the application has no screen for deleting types.
- **Validation:**
  - Data annotations with shared length limits from the `EntityValidation` constants class
  - A custom `MustContainLetter` attribute, with matching server-side and client-side checks, so names like `123` or `!!!` are rejected
  - Service-layer rules reported through `ModelState`: the selected type must exist, an entry name must be unique within its type, and an entry can have at most 20 notes
  - A unique database index on entry name per type

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installed with the Visual Studio ".NET desktop" or "ASP.NET and web development" workloads), or any other SQL Server instance
- The EF Core command-line tool:

  ```
  dotnet tool install --global dotnet-ef
  ```

### Run the project

1. Clone the repository:

   ```
   git clone https://github.com/Brankovanov/scriptorium_nova.git
   cd scriptorium_nova
   ```

2. Restore the dependencies:

   ```
   dotnet restore
   ```

3. Create the database by applying the migrations:

   ```
   dotnet ef database update --project Scriptorium_Nova.Web
   ```

4. Start the application:

   ```
   dotnet run --project Scriptorium_Nova.Web
   ```

5. Open the address printed in the console. With the default launch profile it is `http://localhost:5137` (or `https://localhost:7041` when you use the `https` profile).

The application also applies pending migrations and fills the database with sample data on startup, so step 3 is a safe way to create the database in advance rather than a requirement.

## Configuration

The application needs one setting, the connection string named `DefaultConnection`, in `Scriptorium_Nova.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ScriptoriumNova;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

The default value uses SQL Server LocalDB with Windows authentication, so no username or password is needed for local testing. To use another SQL Server instance, change the value in `appsettings.json`, or override it with an environment variable without editing any file:

```
# PowerShell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=ScriptoriumNova;User Id=...;Password=...;TrustServerCertificate=True;"
```

No other environment variables or credentials are required.

## Seed data

- **Entry types** (added by the migration): Faction, Location, Character, Artifact and Event.
- **Entries and notes:** on the first run, when the Entries table is empty, a seeder adds a set of sample entries spread across all five types, and a few of them also get notes, so the details page is never empty on a fresh database. Once the table contains data, the seeder does nothing, so restarting the application never creates duplicates.

## Possible future features

- User accounts, so each worldbuilder has a private Codex
- Parent and child relations between entries
- Paging on the entries list
- Images and rich text for entries
- Many-to-many tags
