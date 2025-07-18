using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.CQRS.Queries;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers.CookieGlobalVariablesValues;
using MyEverything.ThisMvc.Helpers.DbHelpers;
using MyEverything.ThisMvc.Helpers.Token;
using MyEverything.ThisMvc.Middlewares;
using MyMediatr;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient();

builder.Services.AddScoped<CreateTokensControl>();


builder.AddConnectionString<EverythingDbConnection,EverythingDbContext>();

builder.Services.AddIdentity<AdminLoginInfo, IdentityRole>()
    .AddEntityFrameworkStores<EverythingDbContext>()
    .AddDefaultTokenProviders();


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

 
builder.Services.AddMyMediatr(typeof(AuthorQuery).Assembly);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    
})
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = jwtSettings["Issuer"],
           ValidAudience = jwtSettings["Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(key)
       };
       
       
       // Bu satır, JWT kimlik doğrulama (JwtBearer) mekanizmasının olaylarına (events)
       // müdahale edeceğimizi belirtir.
       options.Events = new JwtBearerEvents
       {
           // OnMessageReceived olayı, kimlik doğrulama mekanizmasının bir istek aldığında
           // İLK olarak tetiklediği olaydır. Herhangi bir doğrulama (validation) yapmadan önce çalışır.
           // Temel görevi, token'ı standart olmayan bir yerden bulup sisteme vermektir.
           OnMessageReceived = context =>
           {

               // 'context' parametresi, o anki HTTP isteği hakkında bilgiler içerir.
               // context.Request.Cookies ile gelen isteğin içindeki tüm cookie'lere erişiriz.
               // ["jwt"] diyerek, adının "jwt" olmasını beklediğimiz cookie'nin değerini almaya çalışırız.
               // Bu "jwt" ismi, token'ı oluştururken cookie'ye verdiğimiz isimle aynı olmalıdır.
               var token = context.Request.Cookies[GlobalCookiesNames.JwtCookieName];

               // Eğer "jwt" isimli bir cookie bulunduysa ve içi boş değilse...
               if (!string.IsNullOrEmpty(token))
               {

                   // İŞTE SİHİR BURADA GERÇEKLEŞİYOR!
                   // 'context.Token' özelliğine, cookie'den okuduğumuz token değerini atarız.
                   // Bu hareketle, kimlik doğrulama mekanizmasına şunu demiş oluyoruz:
                   // "Daha fazla arama yapmana gerek yok, ben token'ı buldum ve sana veriyorum.
                   // Şimdi al bu token'ı ve standart doğrulama adımlarına (imza, süre, issuer kontrolü vb.) devam et."
                   context.Token = token; // burada header gibi davranır
               }

               // Bu olayın bir Task döndürmesi gerekir. İçeride 'await' ile beklediğimiz bir işlem olmadığı için,
               // tamamlanmış bir görevi (Task.CompletedTask) döndürerek işlemi sonlandırırız.
               return Task.CompletedTask;
           },
           OnChallenge =  context =>
           {
               context.HandleResponse();

               
               context.Response.Redirect("/Login/LoginAdmin");

               return Task.CompletedTask;
           },
           OnAuthenticationFailed=context =>
           {
               context.Response.StatusCode = StatusCodes.Status401Unauthorized;
               return Task.CompletedTask;
           }

       };
   }
   


   );



var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
/*app.MapGet("/testconfig", (IOptions<EverythingDbConnection> options) => {
    // IOptions<TDbConnection> doğrudan metoda enjekte edilir ve .Value'su döndürülür.
    return Results.Ok(options.Value);
});*/
app.UseAuthentication();
app.UseMiddleware<RefreshTokenControlMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");




app.Run();