using GSF.FuzzyStrings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        string searchTerm = "halar";

        var categories = SeedData.Categories;
        var closeMatches = categories.Where(c => c.Name.ApproximatelyEquals(searchTerm,
            FuzzyStringComparisonOptions.UseJaccardDistance, FuzzyStringComparisonTolerance.Normal)).ToList();

        return Ok(SeedData.Categories);
    }
}
