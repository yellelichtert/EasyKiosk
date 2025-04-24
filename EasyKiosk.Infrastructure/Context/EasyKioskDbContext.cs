using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Context;

public class EasyKioskDbContext : IdentityDbContext<IdentityUser>
{
    public EasyKioskDbContext(DbContextOptions<EasyKioskDbContext> options) : base(options)
    {
        
    }
    
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Device> Devices { get; set; }


    
    private void SetTimeStamps()
    {
        var changedEntitites = this.ChangeTracker
            .Entries().Where(e =>
                e.Entity is TrackedEntity && (e.State == EntityState.Modified || e.State == EntityState.Added));

        var timeStamp = DateTime.Now;
        foreach (var entry in changedEntitites)
        {
            if (entry.State == EntityState.Added)
            {
                ((TrackedEntity)entry.Entity).CreatedAt = timeStamp;
            }

            ((TrackedEntity)entry.Entity).UpdatedAt = timeStamp;
        }
        
    }

    public override int SaveChanges()
    {
        SetTimeStamps();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetTimeStamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        SetTimeStamps();
        return  await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetTimeStamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

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
            .Property(c => c.Img);
        
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();
        
        modelBuilder.Entity<Category>().Property(c => c.Id).HasColumnOrder(0);
        modelBuilder.Entity<Category>().Property(c => c.Name).HasColumnOrder(2);
        modelBuilder.Entity<Category>().Property(c => c.Img).HasColumnOrder(3);
        
        modelBuilder.Entity<Category>()
            .Property(e => e.CreatedAt)
            .IsRequired();
        
        modelBuilder.Entity<Category>()
            .Property(e => e.UpdatedAt)
            .IsRequired();
        
        
        

        //Product entity.
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .HasColumnType("varchar(255)")
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.Description);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(c => c.Img);

        modelBuilder.Entity<Product>().Property(p => p.Id).HasColumnOrder(0);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasColumnOrder(1);
        modelBuilder.Entity<Product>().Property(p => p.Description).HasColumnOrder(2);
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnOrder(3);
        modelBuilder.Entity<Product>().Property(p => p.CategoryId).HasColumnOrder(4);
        modelBuilder.Entity<Product>().Property(p => p.Img).HasColumnOrder(5);
        
        modelBuilder.Entity<Product>()
            .Property(e => e.CreatedAt)
            .IsRequired();
        
        modelBuilder.Entity<Product>()
            .Property(e => e.UpdatedAt)
            .IsRequired();
        
        
        //Device Entity
        modelBuilder.Entity<Device>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<Device>()
            .Property(d => d.Name)
            .HasColumnType("varchar(20)")
            .IsRequired();

        modelBuilder.Entity<Device>()
            .Property(d => d.DeviceType)
            .HasColumnType("int")
            .IsRequired();

        modelBuilder.Entity<Device>()
            .Property(d => d.Key)
            .IsRequired();

        modelBuilder.Entity<Device>()
            .Property(d => d.IsKeyRevoked)
            .IsRequired();

        modelBuilder.Entity<Device>().Property(d => d.Id).HasColumnOrder(0);
        modelBuilder.Entity<Device>().Property(d => d.DeviceType).HasColumnOrder(1);
        modelBuilder.Entity<Device>().Property(d => d.Key).HasColumnOrder(2);
        modelBuilder.Entity<Device>().Property(d => d.IsKeyRevoked).HasColumnOrder(3);
        
        
        modelBuilder.Entity<Device>()
            .Property(e => e.CreatedAt)
            .IsRequired();
        
        modelBuilder.Entity<Device>()
            .Property(e => e.UpdatedAt)
            .IsRequired();
        
        
        
        //Order entity.
        modelBuilder.Entity<Order>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderDetails)
            .WithOne()
            .HasForeignKey(od => od.OrderId)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .HasOne<Device>()
            .WithMany()
            .HasForeignKey(o => o.DeviceId)
            .IsRequired();
        
        //OrderDetail entity.
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .IsRequired();
        
        modelBuilder.Seed();
        base.OnModelCreating(modelBuilder);
    }
}