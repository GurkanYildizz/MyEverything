using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyEverything.ThisMvc.Helpers.Token
{
    public class ActiveTokenControl
    {
        public  (ClaimsPrincipal, DateTime) TokenActiveControl(string jwt, IConfigurationSection jwtSettings, SymmetricSecurityKey authSigninKey)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigninKey, // Gerçek bir anahtar !
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"], // Token'ı yayınlayan adres
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"], // Token'ın hedef kitlesi
                ValidateLifetime = true, // Süre geçerliliğini kontrol et
                                         // ClockSkew = TimeSpan.Zero // Zaman kayması toleransı (genellikle sıfır veya küçük bir değer)
            };

            SecurityToken validatedToken;
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenControl = tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);
                var tokenRead = tokenHandler.ReadJwtToken(jwt);
                var tokenExpire = tokenRead.ValidTo;

                return (tokenControl, tokenExpire);
            }
            catch (Exception)
            {

                return (null, DateTime.Now.AddMinutes(-15));
            }
        }
    }
}
