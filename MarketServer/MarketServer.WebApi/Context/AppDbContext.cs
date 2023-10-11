﻿using MarketServer.WebApi.Models;
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

}
