using MarketServer.WebApi.Enums;

namespace MarketServer.WebApi.Models;

public sealed class OrderStatues
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public  OrderStatuesEnum Status { get; set; }
    public DateTime StatusDate { get; set; }
}
