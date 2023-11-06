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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]

public sealed class ShoppingCartsController : ControllerBase
{
    [HttpPost]
    public IActionResult Add(AddShoppingCartDto request)
    {
        AppDbContext context = new();
        ShoppingCart cart = new()
        {
            ProductId = request.ProductId,
            Price = request.Price,
            Quantity = 1,
            UserId = request.UserId,
        };
        context.Add(cart);
        context.SaveChanges();

        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult RemoveById(int id)
    {
        AppDbContext context = new();
        var shoppingCart = context.ShoppingCarts.Where(p => p.Id == id).FirstOrDefault();
        if (shoppingCart != null)
        {
            context.Remove(shoppingCart);
            context.SaveChanges();
        }

        return NoContent();
    }

    [HttpGet("{userId}")]
    public IActionResult GetAll(int userId)
    {
        AppDbContext context = new();
        List<ShoppinCartResponseDto> products = context.ShoppingCarts.AsNoTracking().Include(p => p.Product).Select(s => new 
        ShoppinCartResponseDto()
        {
            Id = s.Product.Id,
            Name = s.Product.Name,
            Brand = s.Product.Brand,
            Img = s.Product.Img,
            Description = s.Product.Description,
            Price = s.Price,
            Stock = s.Quantity,
            Barcode = s.Product.Barcode,
            IsActive = s.Product.IsActive,
            CategoryId = s.Product.CategoryId,
            isDelete = s.Product.isDelete,
            ShoppingCartId = s.Id

        }).ToList();

        return Ok(products);
    }

    [HttpPost]
    public IActionResult setShoppingCartsFromLocalStorage(List<SetShoppingCartDto> request) //Birden fazla olabileceği için List yapıldı
    {
        AppDbContext context = new();
        List<ShoppingCart> shoppingCarts = new();

        foreach (var item in request)
        {
            ShoppingCart shoppingCart = new()
            {
                ProductId = item.ProductId,
                UserId = item.UserId,
                Price = item.Price, 
                Quantity = item.Quantity
            };

            shoppingCarts.Add(shoppingCart);
        }

        context.AddRange(shoppingCarts);
        context.SaveChanges();

        return NoContent();
    }

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
            try
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

                //Kullanıcı girişi varsa, işlem bitince sepeti siler
                Models.User user = context.Users.Find(requestDto.UserId);
                if (user is not null)
                {
                    var shoppingCarts = context.ShoppingCarts.Where(p => p.UserId == requestDto.UserId).ToList();
                    context.RemoveRange(shoppingCarts);
                }

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
            }

            //Sipariş alınırken hata oluşursa iade işlemi gerçekleşecek
            catch(Exception ex)
            {
                //Ödeme kırılım ayarı yapılmalı
                CreateRefundRequest refundRequest = new CreateRefundRequest();
                refundRequest.ConversationId = request.ConversationId;
                refundRequest.Locale = Locale.TR.ToString();
                refundRequest.PaymentTransactionId = "1";
                refundRequest.Price = request.Price;
                refundRequest.Ip = "85.34.78.112";
                refundRequest.Currency = currency.ToString();

                Refund refund = Refund.Create(refundRequest, options);

                return BadRequest(new { Message = "İşlem sırasında hata ile karşılaşıldı ve paranızın geri iadesi gerçekleştrildi. Lütfen yapmak istediğiniz işlemi tekrar deneyin." });
            }

            return NoContent();
        }
        else
        {
            return BadRequest(payment.ErrorMessage);
        }
    }
}
