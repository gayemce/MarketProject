namespace MarketServer.WebApi.Models;

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public string Barcode { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; } // Kategoriye ait Id
    public Category Category { get; set; }
}
