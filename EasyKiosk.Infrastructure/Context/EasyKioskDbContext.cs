using EasyKiosk.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Context;

public class EasyKioskDbContext : DbContext
{
    public EasyKioskDbContext(DbContextOptions<EasyKioskDbContext> options) : base(options) {}


    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Category Entity
        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .HasColumnType("varchar(255)")
            .IsRequired();
        
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();


        //Product entity.
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.Description);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        modelBuilder.Entity<Product>().Property(p => p.Id).HasColumnOrder(0);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasColumnOrder(1);
        modelBuilder.Entity<Product>().Property(p => p.Description).HasColumnOrder(2);
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnOrder(3);
        modelBuilder.Entity<Product>().Property(p => p.CategoryId).HasColumnOrder(4);
        
        base.OnModelCreating(modelBuilder);
    }
}