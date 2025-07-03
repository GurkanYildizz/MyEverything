using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.Controllers;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers.CookieGlobalVariablesValues;
using MyEverything.ThisMvc.Helpers.Token;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        var jwt = context.Request.Cookies[GlobalCookiesNames.JwtCookieName];
        var jwtRefresh = context.Request.Cookies[GlobalCookiesNames.RefreshTokenName];

        if (jwt == null || jwtRefresh == null)
        {
            context.Response.Cookies.Delete(GlobalCookiesNames.RefreshTokenName);
            context.Response.Cookies.Delete(GlobalCookiesNames.JwtCookieName);
            await next(context);
            return;
        }


        var userManager = context.RequestServices.GetRequiredService<UserManager<AdminLoginInfo>>();
        var tokenCreateController = context.RequestServices.GetRequiredService<CreateTokensControl>();//servisleri bu şekilde middleware den çağırabiliyoruz...Kendi oluşturduğum servis
        var everythingDbContext = context.RequestServices.GetRequiredService<EverythingDbContext>();
        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

        var jwtSettings = configuration.GetSection("Jwt");
        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        ActiveTokenControl activeTokenControl=new ActiveTokenControl();

        (var tokenActiveControl, var expiration) =activeTokenControl.TokenActiveControl(jwt, jwtSettings, authSigninKey);

       
        if (tokenActiveControl == null)
        {
            context.Response.Cookies.Delete(GlobalCookiesNames.RefreshTokenName);
            context.Response.Cookies.Delete(GlobalCookiesNames.JwtCookieName);
            await next(context);
            return;
        }
        
        var userId = tokenActiveControl.FindFirst("id")?.Value;


        var ownerRefreshToken = await everythingDbContext.UserTokens.FirstOrDefaultAsync(f => f.UserId == userId);

        if (ownerRefreshToken == null)
        {
            context.Response.Cookies.Delete(GlobalCookiesNames.RefreshTokenName);
            context.Response.Cookies.Delete(GlobalCookiesNames.JwtCookieName);
            await next(context);
            return;
        }

        var refreshRecordData = JsonConvert.DeserializeObject<RefershTokenInfo>(ownerRefreshToken.Value);

        if (refreshRecordData.RefreshToken != jwtRefresh || !(refreshRecordData.RefreshTokenExpiration >= DateTime.UtcNow))
        {

            context.Response.Cookies.Delete(GlobalCookiesNames.RefreshTokenName);
            context.Response.Cookies.Delete(GlobalCookiesNames.JwtCookieName);
            everythingDbContext.UserTokens.RemoveRange(ownerRefreshToken);
            everythingDbContext.SaveChanges();
            
            context.Response.Redirect("/Login/LoginAdmin");
            return;
        }

        if ((refreshRecordData.RefreshTokenExpiration - DateTime.UtcNow).Minutes <= 0) //Refreshtoken süresi dolduysa sil
        {
            context.Response.Cookies.Delete(GlobalCookiesNames.RefreshTokenName);
           
            everythingDbContext.UserTokens.RemoveRange(ownerRefreshToken);
            everythingDbContext.SaveChanges();
        }


        if ((expiration - DateTime.UtcNow).Minutes <= 10)
        {


            var findUser = await userManager.FindByIdAsync(userId);

            var tokenData = await tokenCreateController.TokenControls(findUser, new CancellationToken());



            context.Response.Cookies.Append(GlobalCookiesNames.JwtCookieName, tokenData.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = tokenData.AccessTokenExpiration
            });
            context.Response.Cookies.Append(GlobalCookiesNames.RefreshTokenName, tokenData.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = tokenData.RefreshTokenExpiration
            });

        }
        await next(context);
    }

   

    /*
     private DateTime DecodeJwtTokenDate(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(token))
        {
            var jwtToken = handler.ReadJwtToken(token);

            //var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            return (jwtToken.ValidTo);

        }
        return (new DateTime().AddDays(10));
    }
    */
}
