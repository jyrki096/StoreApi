using Api.Service.Payment;

namespace Api.Extension
{
    public static class PaymentServiceExtension
    {
        public static void AddPaymentService(this IServiceCollection services) => services.AddScoped<IPaymentService, FakePaymentService>();
    }
}
