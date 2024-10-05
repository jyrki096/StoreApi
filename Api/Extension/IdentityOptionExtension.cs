using Microsoft.AspNetCore.Identity;

namespace Api.Extension
{
    public static class IdentityOptionExtension
    {
        public static IServiceCollection AddConfigureIdentityOptions(
            this IServiceCollection services)
        {
            return services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });
        }
    }
}