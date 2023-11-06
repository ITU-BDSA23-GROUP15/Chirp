namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
        
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    //     => options.UseSqlite($"Data source={GetDbPath()}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().Property(a => a.AuthorId).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        modelBuilder.Entity<Author>().HasIndex(a => a.AuthorId).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
    }

    // private static string GetDbPath()
    // {
    //     var folder = Environment.SpecialFolder.LocalApplicationData;
    //     var path = Environment.GetFolderPath(folder);
    //     return Path.Join(path, "chirp.db");
    // }
}
