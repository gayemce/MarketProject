using Iyzipay.Model;
using MarketServer.WebApi.Models;

namespace MarketServer.WebApi.Dtos;

public sealed record class PaymentDto(
    int UserId,
    List<Product> Products,
    PaymentCard PaymentCard,
    Buyer Buyer,
    Address ShippingAddress,
    Address BillingAddress); //sepetteki ürünlerin listesi