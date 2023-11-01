namespace MarketServer.WebApi.Dtos;

public sealed record class RegisterDto(
    string Name,
    string Lastname,
    string Email,
    string Password,
    string Username);
