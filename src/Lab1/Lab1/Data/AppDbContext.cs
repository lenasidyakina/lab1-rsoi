using Lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("persons");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(p => p.Age).HasColumnName("age");
            entity.Property(p => p.Address).HasColumnName("address").HasMaxLength(500);
            entity.Property(p => p.Work).HasColumnName("work").HasMaxLength(200);
        });
    }
}