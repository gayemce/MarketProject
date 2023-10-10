namespace MarketServer.WebApi.Models;

public sealed class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool isActive { get; set; }
    public bool isDeleted { get; set; }
}
