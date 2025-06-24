using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEverything.ThisMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AdminLoginInfo> userManager;
        private readonly IConfiguration configuration;
        public AuthController(UserManager<AdminLoginInfo> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            
        }
       

        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] AdminLogin_Dto adminLogin_Dto)
        {
            #region Burası ilk başta admin olmadığı için admin eklemek için geçici çözüm
            /* var newAdminUser = new AdminLoginInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "GurkanYildiz",
                    Email = "gurkan@mail.com",
                    EmailConfirmed = true,

                };
                  await userManager.CreateAsync(newAdminUser, "1q2w3e4R!");
               */ 
            #endregion


            var user = await userManager.FindByEmailAsync(adminLogin_Dto.Email);

           
            

            if (user == null || !(await userManager.CheckPasswordAsync(user, adminLogin_Dto.Password)))
            {

                return StatusCode(StatusCodes.Status401Unauthorized);
                //Burada mesaj gönderilecek şifre yanlış vb...
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            /* foreach (var role in userRoles)
           {
               authClaims.Add(new Claim(ClaimTypes.Role, role));
           }
           */


            var token=GetToken(authClaims);
            return Ok(new LoginResponse_Dto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
            
        }
        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issure"],
                audience: jwtSettings["Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireHour"])),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
