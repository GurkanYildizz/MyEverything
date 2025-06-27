using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyEverything.ThisMvc.Controllers;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers.Token;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyEverything.ThisMvc.Middlewares;

public class RefreshTokenControlMiddleware
{
    private readonly RequestDelegate next;


    public RefreshTokenControlMiddleware(RequestDelegate next)
    {
        this.next = next;

    }
    public async Task InvokeAsync(HttpContext context)
    {

        /*  if (!context.Request.Path.StartsWithSegments("/api")) // Burası gereksiz middleware da sorgu yapmasın diye "/api" ile başlayan istekler geçerli olur
          {
              await next(context);
              return;
          }
        */
        var jwt = context.Request.Cookies["jwt"];
        var jwtRefresh = context.Request.Cookies["jwt-refresh"];

        if (jwt == null || jwtRefresh == null)
        {
            await next(context);
            return;
        }


        var userManager = context.RequestServices.GetRequiredService<UserManager<AdminLoginInfo>>();
        var tokenCreateController = context.RequestServices.GetRequiredService<CreateTokensControl>();//servisleri bu şekilde middleware den çağırabiliyoruz...Kendi oluşturduğum servis
        var everythingDbContext = context.RequestServices.GetRequiredService<EverythingDbContext>();

       
        (var expiration, var email) = DecodeJwtTokenDate(jwt);
        if (email == null)
        {
            await next(context);
            return;
        }
        var findUser = await userManager.FindByEmailAsync(email);
        if (findUser == null)
        {
            await next(context);
            return;
        }
        var userId = findUser.Id;

        var ownerRefreshToken = await everythingDbContext.UserTokens.FirstOrDefaultAsync(f => f.UserId == userId);
        if (ownerRefreshToken == null)
        {
            await next(context);
            return;
        }

        var refreshRecordData = JsonConvert.DeserializeObject<RefershTokenInfo>(ownerRefreshToken.Value);
        if (refreshRecordData.RefreshToken != jwtRefresh || !(refreshRecordData.RefreshTokenExpiration >= DateTime.UtcNow))
        {
            await next(context);
            return;
        }

        if ((expiration - DateTime.UtcNow).Minutes <= 10)
        {
            var user = userManager.FindByEmailAsync(email);

            if (user == null)
            {
                await next(context);
                return;
            }


            var tokenData = await tokenCreateController.TokenControls(await user, new CancellationToken());



            context.Response.Cookies.Append("jwt", tokenData.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = tokenData.AccessTokenExpiration
            });
            context.Response.Cookies.Append("jwt-refresh", tokenData.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = tokenData.RefreshTokenExpiration
            });

        }
        await next(context);
    }
    private (DateTime, string) DecodeJwtTokenDate(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(token))
        {
            var jwtToken = handler.ReadJwtToken(token);
            var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            return (jwtToken.ValidTo, email.Value);

        }
        return (new DateTime().AddDays(10), null);
    }
}
