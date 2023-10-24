using MarketServer.WebApi.ValueObject;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketServer.WebApi.Models;

public sealed class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public Money Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentType { get; set; }
    public string PaymentNumber { get; set; }

    public static string GetNewOrderNumber()
    {
        //Todo: Değiştirlecek.
        return Guid.NewGuid().ToString();
    }
}
