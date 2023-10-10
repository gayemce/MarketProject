using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketServer.WebApi.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : ControllerBase
{

    public ProductsController()
    {
    }

    [HttpPost]
    public IActionResult GetAll(RequestDto request)
    {
        ResponseDto<List<Product>> Response = new();

        var newProducts = new List<Product>();

        if(request.CategoryId != null) 
        {
            newProducts = SeedData.Products
            .Where(p => p.CategoryId == request.CategoryId)
            .ToList();
        }
        else
        {
            newProducts = SeedData.Products;
        }

        newProducts = newProducts
            .Where(x =>
                x.Name.Replace("İ", "i").ToLower().Contains(request.Search.Replace("İ", "i").ToLower()) ||
                x.Brand.Replace("İ", "i").ToLower().Contains(request.Search.Replace("İ", "i").ToLower())
                )
            .ToList();

        Response.Data = newProducts
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
        Response.PageNumber = request.PageNumber;
        Response.PageSize = request.PageSize;
        Response.TotalPageCount = (int)Math.Ceiling(newProducts.Count / (double)request.PageSize);
        Response.IsFirstPage = request.PageNumber == 1;
        Response.IsLastPage = request.PageNumber == Response.TotalPageCount;

        return Ok(Response);
    }

}

public static class SeedData
{
    public static List<Product> Products = new ProductService().CreateSeedProductData();
    public static List<Category> Categories = new ProductService().CreateCategories();
}

public class ProductService
{
    private List<Product> products = new(); //liste oluşturuldu
    private List<Category> categories = new();

    public List<Product> CreateSeedProductData()
    {
        for (int i = 0; i < 100; i++)
        {
            Random random = new();
            var product = new Product()
            {
                Id = i + 1,
                Name = "Pirinç " + (i + 1),
                Brand = "Yayla " + (i + 1),
                Img = "https://market-product-images-cdn.getirapi.com/product/43a3b3b2-7d12-4e16-b72f-2391ad767bd0.jpg",
                Description = "Deneme",
                Price = 3 * (i + 1),
                Stock = i + 1,
                Barcode = "968123456789",
                IsActive = false,
                CategoryId = random.Next(1, 10),
               
            };

            products.Add(product);
        }

        return products;
    }

    public List<Category> CreateCategories()
    {
        var category2 = new Category()
        {
            Id = 0,
            Name = "Halılar",
            isActive = true,
            isDeleted = false
        };

        categories.Add(category2);

        for (int i = 0; i < 10; i++)
        {
            var category = new Category()
            {
                Id = i + 1,
                Name = $"Kategori {i + 1}",
                isActive = true,
                isDeleted = false
            };

            categories.Add(category);
        }

        return categories;
    }
}
