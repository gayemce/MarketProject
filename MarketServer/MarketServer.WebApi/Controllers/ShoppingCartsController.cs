using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using MarketServer.WebApi.Context;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Models;
using MarketServer.WebApi.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class ShoppingCartsController : ControllerBase
{
    [HttpPost]
    public IActionResult Payment(PaymentDto requestDto)
    {
        decimal total = 0;
        decimal comission = 0;
        foreach (var product in requestDto.Products)
        {
            total += product.Price.Value;
        }

        comission = total * 1.2m / 100;

        Currency currency = Currency.TRY;
        string requestCurrency = requestDto.Products[0]?.Price?.Currency;
        if (!string.IsNullOrEmpty(requestCurrency))
        {
            switch (requestCurrency)
            {
                case "₺":
                    currency = Currency.TRY; 
                    break;
                case "$":
                    currency = Currency.USD;
                    break;
                case "£":
                    currency = Currency.USD;
                    break;
                case "€":
                    currency = Currency.EUR;
                    break;
                default:
                    throw new Exception("Para birimi bulunamadı.");
                    break;
            }
        }
        else
        {
            throw new Exception("Sepette ürün bulunamadı!");
        }

        Options options = new Options();
        options.ApiKey = "sandbox-n0iHSihJ3QiTBpPkoZY1eSGxgRFwg5Ij";
        options.SecretKey = "sandbox-YtwDO7drJMVRnTEUMUy4o9ouPRjh2Qb4";
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = total.ToString();
        request.PaidPrice = comission.ToString();
        request.Currency = currency.ToString();
        request.Installment = 1;
        request.BasketId = Order.GetNewOrderNumber(); //GY20230000000001 sipariş numarası
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = requestDto.PaymentCard;
        request.PaymentCard = paymentCard;

        Buyer buyer = requestDto.Buyer;
        buyer.Id = Guid.NewGuid().ToString();
        request.Buyer = buyer;

        Address shippingAddress = requestDto.ShippingAddress;
        Address billingAddress = requestDto.ShippingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();
        foreach (var product in requestDto.Products)
        {
            BasketItem item = new BasketItem();
            item.Id = product.Id.ToString();
            item.Name = product.Name;
            item.ItemType = BasketItemType.PHYSICAL.ToString();
            item.Price = product.Price.Value.ToString();
            basketItems.Add(item);
        }
        request.BasketItems = basketItems;

        Payment payment = Iyzipay.Model.Payment.Create(request, options);
        if(payment.Status == "success")
        {
            List<Order> orders = new();
            foreach (var product in requestDto.Products)
            {
                Order order = new()
                {
                    OrderNumber = request.BasketId,
                    ProductId = product.Id,
                    Price = new Money(product.Price.Value, product.Price.Currency),
                    PaymentDate = DateTime.Now,
                    PaymentType = "Credit Card",
                    PaymentNumber = payment.PaymentId
                };
                orders.Add(order);
            }
            AppDbContext context = new();
            context.Orders.AddRange(orders);
            context.SaveChanges();

            return NoContent();
        }
        else
        {
            return BadRequest(payment.ErrorMessage);
        }

        return NoContent();
    }
}
