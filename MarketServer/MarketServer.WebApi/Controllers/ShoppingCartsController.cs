using Iyzipay.Model;
using Iyzipay.Request;
using MailKit;
using MarketServer.WebApi.Context;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Enums;
using MarketServer.WebApi.Models;
using MarketServer.WebApi.Services;
using MarketServer.WebApi.ValueObject;
using Microsoft.AspNetCore.Mvc;


namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class ShoppingCartsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Payment(PaymentDto requestDto)
    {
        decimal total = 0;
        decimal commission = 0; //komisyon
        foreach (var products in requestDto.Products)
        {
            total += products.Price.Value;
        }
        commission = total;
        //commission = total * 1.2m / 100;

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
                    currency = Currency.GBP;
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
            throw new Exception("Sepette ürünüz yok!");
        }

        //Bağlantı bilgilerini istiyor
        Iyzipay.Options options = new Iyzipay.Options();
        options.ApiKey = "sandbox-n0iHSihJ3QiTBpPkoZY1eSGxgRFwg5Ij";
        options.SecretKey = "sandbox-YtwDO7drJMVRnTEUMUy4o9ouPRjh2Qb4";
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = total.ToString(); //ödeme kısmı
        request.PaidPrice = commission.ToString(); //komisyon + ödeme tutarı
        request.Currency = currency.ToString();
        request.Installment = 1;
        request.BasketId = Order.GetNewOrderNumber();
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = requestDto.PaymentCard;
        request.PaymentCard = paymentCard;

        Buyer buyer = requestDto.Buyer;
        buyer.Id = Guid.NewGuid().ToString();
        request.Buyer = buyer;


        request.ShippingAddress = requestDto.ShippingAddress;
        request.BillingAddress = requestDto.BillingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();
        foreach (var products in requestDto.Products)
        {
            BasketItem item = new BasketItem();
            item.Category1 = "Book";
            item.Category2 = "Book";
            item.Id = products.Id.ToString();
            item.Name = products.Name;
            item.ItemType = BasketItemType.PHYSICAL.ToString();
            item.Price = products.Price.Value.ToString();
            basketItems.Add(item);
        }
        request.BasketItems = basketItems;

        Payment payment = Iyzipay.Model.Payment.Create(request, options);

        if (payment.Status == "success")
        {
            AppDbContext context = new();

            string orderNumber = Order.GetNewOrderNumber();

            List<Order> orders = new();
            foreach (var products in requestDto.Products)
            {
                Order order = new()
                {
                    OrderNumber = orderNumber,
                    ProductId = products.Id,
                    Price = new Money(products.Price.Value, products.Price.Currency),
                    PaymentDate = DateTime.Now,
                    PaymentType = "Credit Cart",
                    PaymentNumber = payment.PaymentId,
                    CreatedAt = DateTime.Now,
                };
                orders.Add(order);
            }

            OrderStatues orderStatues = new()
            {
                OrderNumber = orderNumber,
                Status = OrderStatuesEnum.AwatingApproval,
                StatusDate = DateTime.Now,
            };

            context.OrderStatues.Add(orderStatues);
            context.Orders.AddRange(orders);
            context.SaveChanges();

            //MailService içerisindeki method çağrıldı

            string response = await Services.MailService.SendEmailAsync(requestDto.Buyer.Email, "Siparişiniz Alındı", $@"
            <h1>Siparişiniz Alındı</h1>
            <p>Sipariş Numaranız: {orderNumber}<p>
            <p>Ödeme Numaranız: {payment.PaymentId}<p>
            <p>Ödeme Tutarınız: {payment.PaidPrice}<p>
            <p>Ödeme Tarihiniz: {DateTime.Now}<p>
            <p>Ödeme Tipiniz: Kredi Kartı<p>
            <p>Ödeme Durumunuz: Onay bekliyor<p>");

            return NoContent();
        }
        else
        {
            return BadRequest(payment.ErrorMessage);
        }
    }
}
