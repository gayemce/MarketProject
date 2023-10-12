namespace MarketServer.WebApi.ValueObject;

public sealed class Money
{
    public Money(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public decimal Value { get; private init; }
    public string Currency { get; private init; }
}
