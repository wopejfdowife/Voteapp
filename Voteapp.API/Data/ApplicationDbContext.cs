using Microsoft.EntityFrameworkCore;
using VottingApp.API.Models;

namespace VottingApp.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VoteOption> VoteOptions { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VoteOption>().HasData(
                new VoteOption { Id = 1, Name = "C#", Description = "Modern, powerful programming language", VoteCount = 0, CreatedAt = DateTime.Now },
                new VoteOption { Id = 2, Name = "Python", Description = "Simple and versatile", VoteCount = 0, CreatedAt = DateTime.Now },
                new VoteOption { Id = 3, Name = "Java", Description = "Reliable and scalable", VoteCount = 0, CreatedAt = DateTime.Now },
                new VoteOption { Id = 4, Name = "JavaScript", Description = "The language of the web", VoteCount = 0, CreatedAt = DateTime.Now }
            );
        }
    }
}