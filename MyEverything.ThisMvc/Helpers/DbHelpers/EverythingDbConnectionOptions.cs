using Microsoft.EntityFrameworkCore;

namespace MyEverything.ThisMvc.Helpers.DbHelpers;

public static class EverythingDbConnectionOptions
{
    public static void AddConnectionString<TDbConnection, TDbContext>(this WebApplicationBuilder builder) where TDbConnection : class where TDbContext : DbContext
    {
        builder.Services.Configure<TDbConnection>(builder.Configuration.GetSection(typeof(TDbConnection).Name));

        var connectionString = builder.Configuration.GetSection(typeof(TDbConnection).Name).GetSection("ConnectionString").Value;
        builder.Services.AddDbContext<TDbContext>(option =>
        option.UseSqlServer(connectionString));//dotnet ef migrations add InitialCreate --startup-project MyEverything.ThisMvc
                                               //dotnet ef database update --startup-project MyEverything.ThisMvc
    }
}
