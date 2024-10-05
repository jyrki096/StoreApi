using Api.Common;
using Microsoft.AspNetCore.Identity;

namespace Api.Extension
{
    public static class RoleInitializerServiceExtension
    {
        public static async Task InitializeRoleAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateAsyncScope();
            var roleManager = scope
                            .ServiceProvider
                            .GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in SharedData.Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}