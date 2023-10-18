namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public required DbSet<Cheep> Cheeps { get; set; }
    public required DbSet<Author> Authors { get; set; }

    public string DbPath { get; }

    public ChirpContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().Property(a => a.AuthorId).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
    }
        
}
