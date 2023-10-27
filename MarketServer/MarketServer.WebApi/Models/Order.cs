using MarketServer.WebApi.Context;
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
        string initialLetter = "GY"; //seri numarası
        string year = DateTime.Now.Year.ToString();
        string newOrderNumber = initialLetter + year;

        AppDbContext context = new();
        var lastOrder = context.Orders.OrderByDescending(o => o.Id).FirstOrDefault();
        string currentOrderNumber = lastOrder?.OrderNumber; //son siparişin, sipariş numarası

        if(currentOrderNumber != null)
        {
            string currentYear = currentOrderNumber.Substring(2, 4);
            int startIndex = (currentYear == year) ? 6 : 0; //substring için
            GenerateUniqueOrderNumber(context, ref newOrderNumber, currentOrderNumber.Substring(startIndex));
        }
        else
        {
            newOrderNumber += "0000000001";
        }

        return newOrderNumber;
    }

    private static void GenerateUniqueOrderNumber(AppDbContext context, ref string newOrderNumber, string currentOrderNumStr)
    {
        int currentOrderNumberInt = int.TryParse(currentOrderNumStr, out var num) ? num : 0;
        bool isOrderNumberUnique = false;

        while (!isOrderNumberUnique)
        {
            currentOrderNumberInt++;
            string newOrderNumberTemp = newOrderNumber + currentOrderNumberInt.ToString("D10");
            string checkOrderNumber = newOrderNumberTemp;
            var order = context.Orders.FirstOrDefault(o => o.OrderNumber == checkOrderNumber);
            if(order == null)
            {
                newOrderNumber = newOrderNumberTemp;
                isOrderNumberUnique = true;
            }
        }
    }

}
