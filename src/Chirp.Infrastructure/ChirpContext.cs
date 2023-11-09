namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Follower> Followers { get; set; }

    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Authors
        modelBuilder.Entity<Author>().Property(a => a.AuthorId).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        modelBuilder.Entity<Author>().HasIndex(a => a.AuthorId).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
        //Followers
        modelBuilder.Entity<Follower>().HasIndex(a => a.FollowerId).IsUnique();
        modelBuilder.Entity<Follower>().HasIndex(a => a.FollowingId).IsUnique();
    }
}
