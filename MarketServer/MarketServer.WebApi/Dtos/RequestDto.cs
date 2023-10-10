namespace MarketServer.WebApi.Dtos;

public sealed record RequestDto(
    int PageNumber,
    int PageSize,
    string Search,
    int? CategoryId);
