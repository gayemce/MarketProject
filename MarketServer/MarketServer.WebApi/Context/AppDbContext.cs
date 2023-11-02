using MarketServer.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketServer.WebApi.Context;

public sealed class AppDbContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-I7G56NT\\SQLEXPRESS;Initial Catalog=MarketProjectDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatues> OrderStatues { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //One-to-Many" (Bire-Çok) ilişkiyi Fluent API kullanarak konfigüre etme işlemi
        modelBuilder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);

        //Value Objects
        modelBuilder.Entity<Product>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });

        modelBuilder.Entity<Order>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });

        modelBuilder.Entity<ShoppingCart>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });

        //OrderStatues tablosunda dublicate önlendi
        modelBuilder.Entity<OrderStatues>().HasIndex(p => new { p.Status, p.OrderNumber }).IsUnique();

        //Users tablosunda dublicate önlendi
        modelBuilder.Entity<User>().HasIndex(p => new {p.Email, p.Username}).IsUnique();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Makarna", isActive = true, isDeleted = false },
            new Category { Id = 2, Name = "Pirinç", isActive = true, isDeleted = false },
            new Category { Id = 3, Name = "Bulgur", isActive = true, isDeleted = false },
            new Category { Id = 4, Name = "Bakliyat", isActive = true, isDeleted = false },
            new Category { Id = 5, Name = "Salça", isActive = true, isDeleted = false },
            new Category { Id = 6, Name = "Sos", isActive = true, isDeleted = false },
            new Category { Id = 7, Name = "Konserve", isActive = true, isDeleted = false },
            new Category { Id = 8, Name = "Un", isActive = true, isDeleted = false },
            new Category { Id = 9, Name = "Sıvı Yağ", isActive = true, isDeleted = false },
            new Category { Id = 10, Name = "Zeytinyağı", isActive = true, isDeleted = false },
            new Category { Id = 11, Name = "Şeker", isActive = true, isDeleted = false },
            new Category { Id = 12, Name = "Sirke & Salata Sosu", isActive = true, isDeleted = false },
            new Category { Id = 13, Name = "Baharat", isActive = true, isDeleted = false },
            new Category { Id = 14, Name = "Çorba", isActive = true, isDeleted = false },
            new Category { Id = 15, Name = "Tatlı", isActive = true, isDeleted = false },
            new Category { Id = 16, Name = "Pasta Malzemeleri", isActive = true, isDeleted = false },
            new Category { Id = 17, Name = "Krema", isActive = true, isDeleted = false },
            new Category { Id = 18, Name = "Terayağ & Margarin", isActive = true, isDeleted = false });
    }

}
