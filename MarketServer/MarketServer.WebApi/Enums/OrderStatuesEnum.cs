namespace MarketServer.WebApi.Enums;

public enum OrderStatuesEnum
{
    AwatingApproval = 0, //Onay bekliyor
    BeingPrepared = 1, //Hazırlanıyor
    InTransit = 2, //Taşınma aşamasında
    Deliverid = 3, //Teslim edildi
    Rejected = 4, //Reddedildi
    Returned = 5, //İade edildi
}
