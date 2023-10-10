using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class ChirpContext : DbContext 
{
    public DbSet<Cheep> cheeps {get; set;}
    public DbSet<Author> authors {get; set;}

    public string DbPath{get;}

    public ChirpContext() 
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "chirp.db");
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
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public Author Author { get; set; }
}