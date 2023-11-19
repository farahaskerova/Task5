using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Entity;
using System.Reflection.Emit;
using Task.Contracts;
using Task.Database.DomainModels;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Task.Database;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
          .Entity<Category>()
          .ToTable("categories", t => t.ExcludeFromMigrations());

        base.OnModelCreating(modelBuilder);
    }



    public DbSet<Category> Categories { get; set; }
}

