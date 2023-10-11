using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public string DbPath { get; }

    public ChirpContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data source={DbPath}");
}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}

public class Cheep
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}