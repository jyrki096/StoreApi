using Api.Service;

namespace Api.Extension
{
    public static class OrderServiceExtension
    {
        public static IServiceCollection AddOrdersService(this IServiceCollection services) => services.AddScoped<OrderService>();
    }
}