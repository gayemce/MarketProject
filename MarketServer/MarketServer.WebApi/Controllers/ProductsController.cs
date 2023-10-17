using MarketServer.WebApi.Context;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketServer.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public IActionResult GetAll(RequestDto request)
    {
        AppDbContext context = new();

        List<Product> products = new();

        if(request.CategoryId == null) //hepsini getir
        {
            products = context.Products
                .Where(p => p.IsActive == true && p.isDelete == false)
                .Where(p => p.Name.ToLower().Contains(request.Search.ToLower()) || p.Brand.ToLower().Contains(request.Search.ToLower()))
                .Take(request.PageSize) //kadarını alsın
                .ToList();
        }
        else
        {
             products = context.Products
                .Where(p => p.CategoryId == request.CategoryId)
                .Where(p => p.Name.ToLower().Contains(request.Search.ToLower()) || p.Brand.ToLower().Contains(request.Search.ToLower()))
                .Take(request.PageSize)
                .ToList();
        }

        return Ok(products);
    }
}

