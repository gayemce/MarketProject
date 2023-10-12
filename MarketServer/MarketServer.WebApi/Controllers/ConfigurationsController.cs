﻿using MarketServer.WebApi.Context;
using MarketServer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ConfigurationsController : ControllerBase
{
    private readonly AppDbContext context = new(); //Databese çağrıldı

    [HttpGet]
    public IActionResult SeedData()
    {
        List<Category> categories = new();
        for (int i = 0; i < 10; i++)
        {
            var category = new Category()
            {
                //Id kendi türetebilir.
                Name = $"Category {i}",
                isActive = true,
                isDeleted = false
            };
            categories.Add(category);
        }

        context.Categories.AddRange(categories);
        context.SaveChanges();

        List<Product> products = new();
        for (int i = 0; i < 100; i++)
        {
            Random random = new();
            var product = new Product()
            {
                //Id kendi türetebilir.
                Name = "Pirinç " + (i + 1),
                Brand = "Yayla " + (i + 1),
                Img = "https://market-product-images-cdn.getirapi.com/product/43a3b3b2-7d12-4e16-b72f-2391ad767bd0.jpg",
                Description = "Deneme",
                Price = new(i*2, "₺"),
                Stock = i + 1,
                Barcode = "968123456789",
                IsActive = false,
                CategoryId = random.Next(1, 10),

            };

            products.Add(product);
        }
        context.Products.AddRange(products);
        context.SaveChanges();

        return NoContent();
    }
}