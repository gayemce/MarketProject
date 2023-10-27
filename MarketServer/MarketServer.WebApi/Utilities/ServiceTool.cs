namespace MarketServer.WebApi.Utilities;

public static class ServiceTool
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static IServiceProvider CreateServiceTool(this IServiceCollection services)
    {
        ServiceProvider = services.BuildServiceProvider();
        return ServiceProvider;
    }
}
