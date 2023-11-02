namespace MarketServer.WebApi.Dtos;

public sealed record LoginResponseDto(
    string Token,
    int UserId,
    string UserName);
