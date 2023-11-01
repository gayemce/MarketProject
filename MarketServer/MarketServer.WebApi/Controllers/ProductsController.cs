using AutoMapper;
using MarketServer.WebApi.Context;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketServer.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public ProductsController(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpPost]
    public IActionResult GetAll(RequestDto request)
    {

        List<Product> products = new();

        if(request.CategoryId == null) //hepsini getir
        {
            products = _context.Products
                .Where(p => p.IsActive == true && p.isDelete == false)
                .Where(p => p.Name.ToLower().Contains(request.Search.ToLower()) || p.Brand.ToLower().Contains(request.Search.ToLower()))
                .Take(request.PageSize) //kadarını alsın
                .ToList();
        }
        else
        {
             products = _context.Products
                .Where(p => p.CategoryId == request.CategoryId)
                .Where(p => p.Name.ToLower().Contains(request.Search.ToLower()) || p.Brand.ToLower().Contains(request.Search.ToLower()))
                .Take(request.PageSize)
                .ToList();
        }

        List<ProductDto> requestDto = new();
        foreach (var product in products)
        {
            ProductDto productDto = _mapper.Map<ProductDto>(product);
            productDto.Categories = _context.Categories
                            .Where(c => c.ProductId == product.Id)
                            .Select(c => c.Name) // Sadece kategori adlarını al
                            .ToList();

            //var productDto = new ProductDto()
            //{
            //    Name = product.Name,
            //    Brand = product.Brand,
            //    Img = product.Img,
            //    Description = product.Description,
            //    Price = product.Price,
            //    Stock = product.Stock,
            //    Barcode = product.Barcode,
            //    IsActive = product.IsActive,
            //    isDelete = product.isDelete,
            //    Categories = _context.Categories
            //                .Where(c => c.ProductId == product.Id)
            //                .Select(c => c.Name) // Sadece kategori adlarını al
            //                .ToList(),
            //};
            requestDto.Add(productDto);
        }

        return Ok(requestDto);

    }
}

