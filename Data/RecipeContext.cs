using Microsoft.EntityFrameworkCore;
using RecipeApi.Models;

namespace RecipeApi.Data;

public class RecipeContext : DbContext
{
    public RecipeContext(DbContextOptions<RecipeContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Recipe> Recipe { get; set; }
    public DbSet<Note> Note { get; set; }
    public DbSet<RecipeUser> RecipeUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(p => p.Users)
            .WithMany(p => p.Recipes)
            .UsingEntity<RecipeUser>(
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.RecipeUsers)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Recipe)
                    .WithMany(p => p.RecipeUsers)
                    .HasForeignKey(pt => pt.RecipeId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.RecipeId });
                });
    }
}