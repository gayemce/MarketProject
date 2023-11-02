using MarketServer.WebApi.ValueObject;

namespace MarketServer.WebApi.Dtos;

public sealed record SetShoppingCartDto(
    int ProductId,
    int UserId,
    int Quantity,
    Money Price);
