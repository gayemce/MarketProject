using MarketServer.WebApi.Context;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(CreateCategoryDto request)
    {
        AppDbContext context = new();

        var checkNameIsUnique = context.Categories.Any(p => p.Name == request.Name); //true or false
        if (checkNameIsUnique)
        {
            return BadRequest("Kategori adı daha önce kullanılmıştır.");
        }

        Category category = new()
        {
            Name = request.Name,
            isActive = true,
            isDeleted = false
        };

        context.Categories.Add(category);
        context.SaveChanges();
        return Ok(category); //Best practice for Create
    }

    [HttpGet("{id}")]
    public IActionResult RemoveById(int id) 
    {
        AppDbContext context = new();

        var category = context.Categories.Find(id);
        if(category == null)
        {
            return NotFound();
        }
        category.isDeleted = true;
        context.SaveChanges();
        //return Ok(GetAllCategory());
        return NoContent();

    }

    [HttpPost]
    public IActionResult Update(UpdateCategoryDto request)
    {
        AppDbContext context = new();
        var category = context.Categories.Find(request.Id);
        if(category == null)
        {
            return NotFound();
        }
        category.Name = request.Name;
        context.SaveChanges();
        return NoContent();

    }

    [HttpGet]
    public IActionResult GetAll()
    {
        AppDbContext context = new();

        var categories =
            context.Categories
            .Where(p => p.isActive == true && p.isDeleted == false)
            .OrderBy(o => o.Name).ToList();

        return Ok(categories);
    }
}
