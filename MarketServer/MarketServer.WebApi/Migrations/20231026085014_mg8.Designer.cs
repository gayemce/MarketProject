﻿// <auto-generated />
using System;
using MarketServer.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarketServer.WebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231026085014_mg8")]
    partial class mg8
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarketServer.WebApi.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Makarna",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 2,
                            Name = "Pirinç",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bulgur",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 4,
                            Name = "Bakliyat",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 5,
                            Name = "Salça",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 6,
                            Name = "Sos",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 7,
                            Name = "Konserve",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 8,
                            Name = "Un",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 9,
                            Name = "Sıvı Yağ",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 10,
                            Name = "Zeytinyağı",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 11,
                            Name = "Şeker",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 12,
                            Name = "Sirke & Salata Sosu",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 13,
                            Name = "Baharat",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 14,
                            Name = "Çorba",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 15,
                            Name = "Tatlı",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 16,
                            Name = "Pasta Malzemeleri",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 17,
                            Name = "Krema",
                            isActive = true,
                            isDeleted = false
                        },
                        new
                        {
                            Id = 18,
                            Name = "Terayağ & Margarin",
                            isActive = true,
                            isDeleted = false
                        });
                });

            modelBuilder.Entity("MarketServer.WebApi.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MarketServer.WebApi.Models.OrderStatues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("OrderStatues");
                });

            modelBuilder.Entity("MarketServer.WebApi.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Stock")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("isDelete")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MarketServer.WebApi.Models.Order", b =>
                {
                    b.HasOne("MarketServer.WebApi.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("MarketServer.WebApi.ValueObject.Money", "Price", b1 =>
                        {
                            b1.Property<int>("OrderId")
                                .HasColumnType("int");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("nvarchar(5)");

                            b1.Property<decimal>("Value")
                                .HasColumnType("money");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Price")
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MarketServer.WebApi.Models.Product", b =>
                {
                    b.HasOne("MarketServer.WebApi.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("MarketServer.WebApi.ValueObject.Money", "Price", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("nvarchar(5)");

                            b1.Property<decimal>("Value")
                                .HasColumnType("money");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Category");

                    b.Navigation("Price")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
