using MarketServer.WebApi.ValueObject;

namespace MarketServer.WebApi.Dtos;

public sealed record AddShoppingCartDto(
    int ProductId,
    Money Price,
    int Quantity,
    int UserId);
