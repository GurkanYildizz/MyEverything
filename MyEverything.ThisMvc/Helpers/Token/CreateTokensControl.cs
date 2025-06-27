using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyEverything.ThisMvc.Helpers.Token;
public record RefershTokenInfo(string RefreshToken, DateTime RefreshTokenExpiration);
public class CreateTokensControl

{
    private readonly EverythingDbContext everythingDbContext;
    private readonly IConfiguration configuration;
    public CreateTokensControl()
    {

    }
    public CreateTokensControl(EverythingDbContext everythingDbContext, IConfiguration configuration)
    {
        this.everythingDbContext = everythingDbContext;
        this.configuration = configuration;
    }
   
    public async Task<LoginResponse_Dto> TokenControls(AdminLoginInfo user, CancellationToken cancellationToken)
    { //-----------------------------------------
       
        var refreshToken = $"{Guid.NewGuid().ToString()}{DateTime.Now.ToString()}";
        var refreshTokenExpiration = DateTime.Now.AddDays(7);
        var oldRefreshToken = await everythingDbContext.UserTokens.FirstOrDefaultAsync(f => f.UserId == user.Id);
        if (oldRefreshToken == null)
        {
            everythingDbContext.UserTokens.Add(new IdentityUserToken<string>
            {
                UserId = user.Id,
                Name = "gurkan",
                Value = JsonConvert.SerializeObject(new RefershTokenInfo(
                RefreshToken: refreshToken,
                RefreshTokenExpiration: refreshTokenExpiration
            )),
                LoginProvider = ""

            });
        }
        else
        {
            oldRefreshToken.Value = JsonConvert.SerializeObject(new RefershTokenInfo (
                RefreshToken : refreshToken,
                RefreshTokenExpiration : refreshTokenExpiration
            ));


        }
        await everythingDbContext.SaveChangesAsync(cancellationToken);

        //-------------------------------------------------------

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };


        /* foreach (var role in userRoles)
      {
          authClaims.Add(new Claim(ClaimTypes.Role, role));
      }
      */



        var token = GetAccessToken(authClaims);
        var addLoginResponse = new LoginResponse_Dto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            AccessTokenExpiration = token.ValidTo,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiration

        };
        return addLoginResponse;
    }
    private JwtSecurityToken GetAccessToken(List<Claim> claims)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var token = new JwtSecurityToken(
            issuer: jwtSettings["issuer"],
            audience: jwtSettings["Audience"],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinute"])),
            claims: claims,           

            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
