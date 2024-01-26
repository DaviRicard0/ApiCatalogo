using DesafioAnotaAi.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace DesafioAnotaAi.Context;

public class ApiContext : DbContext
{
    public DbSet<Product> Products { get; init; }
    public DbSet<Category> Categories { get; init; }
    public DbSet<Owner> Owners { get; init; }

    public static ApiContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<ApiContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    public ApiContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().ToCollection("products");
        modelBuilder.Entity<Category>().ToCollection("categories");
        modelBuilder.Entity<Owner>().ToCollection("owners");
    }
}
