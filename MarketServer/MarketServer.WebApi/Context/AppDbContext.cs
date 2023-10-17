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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });


        //Seed Data: Development sürecinde elinde veri olmasını sağlar ki üzerinde çalışılabilsin.
        //Canlıya aldığında değişmeyecek ve database de kayıt olarak tutman gereken verilerin olmasını sağlar.

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
