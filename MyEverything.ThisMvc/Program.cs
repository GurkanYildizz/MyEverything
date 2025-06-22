using Microsoft.Extensions.Options;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers.DbHelpers;
using System.ComponentModel.Design.Serialization;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();



builder.AddConnectionString<EverythingDbConnection,EverythingDbContext>();



var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
/*app.MapGet("/testconfig", (IOptions<EverythingDbConnection> options) => {
    // IOptions<TDbConnection> doğrudan metoda enjekte edilir ve .Value'su döndürülür.
    return Results.Ok(options.Value);
});*/
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");




app.Run();