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
    private readonly AppDbContext _context;

    public ShoppingCartsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{productId}/{quantity}")]
    public IActionResult CheckProductQuantityIsAvailable(int productId, int quantity)
    {
        Product product = _context.Products.Find(productId);
        if (product.Stock < quantity)
        {
            throw new Exception("Stokta bu kadar adet kitap yok!");
        }

        return NoContent();
    }

    [HttpGet("{productId}/{quantity}")]
    public IActionResult ChangeProductQuantityInShoppingCart(int productId, int quantity)
    {
        ShoppingCart cart = _context.ShoppingCarts.Where(p => p.ProductId == productId).FirstOrDefault();

        if(cart is null)
        {
            throw new Exception("Ürün sepette bulunamadı!");
        }

        Product product = _context.Products.Find(productId);

        if (quantity <= 0)
        {
            //product.Stock += 1;

            _context.Remove(cart);
            //_context.Update(product);
        }

        else
        {
            cart.Quantity = quantity;
            
            if(product.Stock < cart.Quantity)
            {
                throw new Exception("Stokta bu kadar adet kitap yok!");
            }

            _context.Update(cart);

        }

        _context.SaveChanges();

        return NoContent();

    }

    [HttpPost]
    public IActionResult Add(AddShoppingCartDto request)
    {
        Product product = _context.Products.Find(request.ProductId);
        if(product is null)
        {
            throw new Exception("Ürün bulunamadı!");
        }

        if(product.Stock < request.Quantity)
        {
            throw new Exception("Ürün stokta kalmadı!");
        }

        
        ShoppingCart cart = 
            _context.ShoppingCarts
            .Where(p => p.ProductId == request.ProductId)
            .FirstOrDefault();

        //eklenen ürün daha önceden sepette mevcutsa bir artır
        if (cart is not null)
        {
            cart.Quantity += 1;

            _context.Update(product);
        }
        //değilse direkt 1 adet ekle
        else
        {
            cart = new()
            {
                ProductId = request.ProductId,
                Price = request.Price,
                Quantity = 1,
                UserId = request.UserId,
            };

            _context.Add(cart);

        }

        _context.SaveChanges();

        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult RemoveById(int id)
    {
        var shoppingCart = _context.ShoppingCarts.Where(p => p.Id == id).FirstOrDefault();
        if (shoppingCart != null)
        {
            //Product product = _context.Products.Find(shoppingCart.ProductId);
            //product.Stock += shoppingCart.Quantity;

            //_context.Update(product);

            _context.Remove(shoppingCart);
            _context.SaveChanges();
        }

        return NoContent();
    }

    [HttpGet("{userId}")]
    public IActionResult GetAll(int userId)
    {
        List<ShoppinCartResponseDto> products = _context.ShoppingCarts.AsNoTracking().Include(p => p.Product).Select(s => new 
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

        _context.AddRange(shoppingCarts);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Payment(PaymentDto requestDto)
    {
        //ödeme esnasında stoğu tekrar kontrol eder
        foreach(var item in requestDto.Products)
        {
            Product checkProduct = _context.Products.Find(item.Id);
            if(checkProduct.Stock < item.Stock)
            {
                throw new Exception($"{item.Name} Ürün stokta kalmadı!");
            }
        }

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
            item.Category1 = "Product";
            item.Category2 = "Product";
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

                string orderNumber = Order.GetNewOrderNumber();

                List<Order> orders = new();
                foreach (var products in requestDto.Products)
                {

                    //Ödeme yapıldıktan sonra stoğu günceller
                    Product changeBookQuantity = _context.Products.Find(products.Id);
                    changeBookQuantity.Stock -= products.Stock;
                    _context.Update(changeBookQuantity);

                    Order order = new()
                    {
                        OrderNumber = orderNumber,
                        ProductId = products.Id,
                        Quantity = (int)products.Stock,
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


                _context.OrderStatues.Add(orderStatues);
                _context.Orders.AddRange(orders);

                //Kullanıcı girişi varsa, işlem bitince sepeti siler
                Models.User user = _context.Users.Find(requestDto.UserId);
                if (user is not null)
                {
                    var shoppingCarts = _context.ShoppingCarts.Where(p => p.UserId == requestDto.UserId).ToList();
                    _context.RemoveRange(shoppingCarts);
                }

                _context.SaveChanges();

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
