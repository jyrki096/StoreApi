using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Extension
{
    public static class PostgreSqlServiceExtension
    {
        public static void AddPostgreSqlDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
               // configuration.GetConnectionString("PostgreSQLConnection")));
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("PostgreSQLConnection"));
            dataSourceBuilder.MapEnum<OrderStatus>();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dataSource));
        }

        public static void AddPostgreSqlIdentityContext(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }
    }
}