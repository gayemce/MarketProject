using System.ComponentModel.DataAnnotations.Schema;

namespace MarketServer.WebApi.Models;

public sealed class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool isActive { get; set; }
    public bool isDeleted { get; set; }
    public int ProductId { get; set; } // Ürüne ait Id
    public ICollection<Product> Products { get; set; } // Her kategorinin birden fazla ürünü olabilir
}
